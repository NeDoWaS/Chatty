using ChattyServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChattyServer
{
    public class Client
    {
        public string UserName { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            UserName = _packetReader.ReadMessage();

            Console.WriteLine($"UserName: [ {UserName} ] kliyent konnect. Time: {DateTime.Now}");
        }
    }
}
