using Objects.Global;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using TelnetCommunication.Interface;
using static Objects.Global.Logging.LogSettings;

namespace ServerTelnetCommunication
{
    [ExcludeFromCodeCoverage]
    public class ConnectionHandler
    {
        public bool ContinueRunning { get; set; }
        public ConnectionHandler(IMudMessage mudMessage)
        {
            ContinueRunning = true;
            TcpClient clientSocket = default(TcpClient);
            TcpListener serverSocket = new TcpListener(IPAddress.Any, GlobalReference.GlobalValues.Settings.Port);
            serverSocket.Start();

            while (ContinueRunning)
            {
                clientSocket = serverSocket.AcceptTcpClient();
                LogConnection(clientSocket);
                clientSocket.ReceiveBufferSize = (int)Math.Pow(2, 20);
                clientSocket.SendBufferSize = clientSocket.ReceiveBufferSize;
                ServerHandler serverHandler = new ServerHandler(clientSocket, mudMessage);
            }

            clientSocket.Close();
            serverSocket.Stop();
        }

        private static void LogConnection(TcpClient clientSocket)
        {
            string line = string.Format("Client connected from {0}.", clientSocket.Client.RemoteEndPoint.ToString());
            GlobalReference.GlobalValues.Logger.Log(LogLevel.ALL, line);
        }
    }
}
