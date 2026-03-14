using ChattyClient.Net.IO;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChattyClient.Net
{
    class Server
    {
        TcpClient _client;

        public PacketReader PacketReader;

        public event Action connectedEvent;
        public event Action usrDisconnectedEvent;
        public event Action msgRecievedEvent;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string username) 
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    var connectPacket = new PacketBuilder();
                    connectPacket.WriteOpCode(0);
                    connectPacket.WriteMessage(username.ToString());
                    _client.Client.Send(connectPacket.GetPacketBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = PacketReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;

                        case 5: 
                            msgRecievedEvent?.Invoke();
                            break;

                        case 10:
                            usrDisconnectedEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine("Ya upal... \n Chto to slomalos...");
                            break;
                    }
                }
            });
        }

        public void SendMessageToServer(string message)
        {
            if (_client.Connected)
            {
                var messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);
                _client.Client.Send(messagePacket.GetPacketBytes());
            }
        }
    }
}
