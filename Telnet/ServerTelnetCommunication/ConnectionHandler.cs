using Objects.Global;
using Objects.Global.FileIO.Interface;
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
            TcpClient clientSocket = null!;
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

            if (GlobalReference.GlobalValues.Settings.UseCachingFileIO)
            {
                ICachedFileIO cachedFileIO = GlobalReference.GlobalValues.FileIO as ICachedFileIO;
                cachedFileIO.Flush();
            }
        }

        private static void LogConnection(TcpClient clientSocket)
        {
            string line = string.Format("Client connected from {0}.", clientSocket.Client.RemoteEndPoint.ToString());
            GlobalReference.GlobalValues.Logger.Log(LogLevel.ALL, line);
        }
    }
}
