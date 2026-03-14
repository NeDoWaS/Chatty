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

            Task.Run (() => Process());
        }

        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var message = _packetReader.ReadMessage();
                            Console.WriteLine($"Time: [{DateTime.Now}] Username: ({UserName}). Message recieved, content: \"{message}\"");
                            Program.BroadcastMessage($"[{DateTime.Now}] - [{UserName}]: {message}");
                            break;

                        case 10:
                            Program.BroadcastDisconnectMessage(UID.ToString()); 
                            break;

                        default:
                            Console.WriteLine("Ya upal... \n Chto to slomalos...");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Random rnd = new Random();
                    Console.WriteLine($"User {UID.ToString()} has been taken to the dark alley and shot in the back {rnd.Next(0,100)} times");
                    Program.BroadcastDisconnectMessage(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
