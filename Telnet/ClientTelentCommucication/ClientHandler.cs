using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Threading;
using TelnetCommunication;
using TelnetCommunication.Interface;

namespace ClientTelentCommucication
{
    [ExcludeFromCodeCoverage]
    public class ClientHandler : TelnetHandler
    {
        public ClientHandler(string ipAddress, int port, IMudMessage message) : base(message)
        {
            _guid = Guid.NewGuid().ToString();
            _clientSocket = new TcpClient();
            _clientSocket.ReceiveBufferSize = (int)Math.Pow(2, 20);
            _clientSocket.SendBufferSize = _clientSocket.ReceiveBufferSize;
            _clientSocket.Connect(ipAddress, port);

            Thread messageLoop = new Thread(GetMessageLoop);
            messageLoop.IsBackground = true;
            messageLoop.Name = "GetMessageLoop";
            messageLoop.Start();

            messageLoop = new Thread(SendMessageLoop);
            messageLoop.IsBackground = true;
            messageLoop.Name = "SendMessageLoop";
            messageLoop.Start();
        }
    }
}