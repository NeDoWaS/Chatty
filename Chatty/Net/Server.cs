using ChattyClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChattyClient.Net
{
    public class Server
    {
        TcpClient _client;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username) 
        {
            if(!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                var connectPacket = new PacketBuilder();
                connectPacket.WriteOpCode(0);
                connectPacket.WriteString(username);
                _client.Client.Send(connectPacket.GetPacketBytes());
            }
        }
    }
}
