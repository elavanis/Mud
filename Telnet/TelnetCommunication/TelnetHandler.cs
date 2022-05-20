using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TelnetCommunication.Interface;

namespace TelnetCommunication
{
    [ExcludeFromCodeCoverage]
    public abstract class TelnetHandler
    {
        private int disconnectedClientCount = 0;

        public ConcurrentQueue<string> InQueue { get; set; } = new ConcurrentQueue<string>();
        public ConcurrentQueue<string> OutQueue { get; set; } = new ConcurrentQueue<string>();

        public IMudMessage MudMessageInstance { get; }

        protected TcpClient ClientSocket { get; }
        protected string GuidString { get; }

        public TelnetHandler(TcpClient tcpClient, IMudMessage mudMessage, string guid)
        {
            ClientSocket = tcpClient;
            MudMessageInstance = mudMessage;
            GuidString = guid;
        }

        public bool Connected
        {
            get
            {
                return ClientSocket.Connected;
            }
        }

        private string _previousPartialMessage = "";

        protected void GetMessageLoop()
        {
            try
            {
                while ((ClientSocket.Connected))
                {
                    //Handle message from client
                    try
                    {
                        GetMessage();
                    }
                    catch /*(Exception ex)*/
                    {
                        //if (ex.InnerException != null && ex.InnerException.Message != "A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond")
                        //{
                        //    throw;
                        //}
                        throw;
                    }
                }

                if (!ClientSocket.Connected)
                {
                    throw new Exception("Server shutdown.");
                }
            }
            catch (Exception ex)
            {
                InQueue.Enqueue("<Exception> " + ex.ToString() + " </Exception>");
                ClientSocket.Close();
            }
        }

        protected void SendMessageLoop()
        {
            try
            {
                while ((ClientSocket.Connected))
                {
                    while (OutQueue.TryDequeue(out string? outboundMessage))
                    {
                        SendMessage(outboundMessage);
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                InQueue.Enqueue(" >> " + ex.ToString());
            }
        }

        private void GetMessage()
        {
            byte[] bytesFrom = new byte[ClientSocket.ReceiveBufferSize];

            ClientSocket.Client.Receive(bytesFrom);

            string dataFromClient = Encoding.Unicode.GetString(bytesFrom);

            lock (_previousPartialMessage)
            {
                DetectDisconnectedClient(dataFromClient);
                try
                {
                    string completeMessage = _previousPartialMessage + dataFromClient;
                    //string delimieter = MudMessageInstance.Regex;
                    //string[] sections = completeMessage.Split(new[] { delimieter }, StringSplitOptions.RemoveEmptyEntries);

                    Tuple<List<string>, string> result = MudMessageInstance.ParseRawMessage(completeMessage);
                    foreach (string message in result.Item1)
                    {
                        if (message == "<Info>You have been successfully logged out.</Info>")
                        {
                            ClientSocket.Close();
                        }

                        InQueue.Enqueue(message);
                    }
                    _previousPartialMessage = result.Item2;
                }
                catch /*(Exception ex)*/
                {
                    _previousPartialMessage += dataFromClient;
                    _previousPartialMessage = _previousPartialMessage.Replace("\0", "");
                }
            }
        }

        private void DetectDisconnectedClient(string dataFromClient)
        {
            string newMessage = dataFromClient.Replace("\0", "");
            if (string.IsNullOrEmpty(newMessage))
            {
                disconnectedClientCount++;
            }
            else
            {
                disconnectedClientCount = 0;
            }

            if (disconnectedClientCount > 100)
            {
                throw new SocketException();
            }
        }

        private void SendMessage(string outboundMessage)
        {
            IMudMessage message = MudMessageInstance.CreateNewInstance(GuidString, outboundMessage);

            byte[] sendBytes = Encoding.Unicode.GetBytes(message.Serialize());
            NetworkStream networkStream = ClientSocket.GetStream();
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
        }
    }
}
