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
        public ClientHandler(string ipAddress, int port, IMudMessage message) : base(new TcpClient(), message, Guid.NewGuid().ToString())
        {
            ClientSocket.ReceiveBufferSize = (int)Math.Pow(2, 20);
            ClientSocket.SendBufferSize = ClientSocket.ReceiveBufferSize;
            ClientSocket.Connect(ipAddress, port);

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