using ChattyClient.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
namespace ChattyServer
{
    class Program ()
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                var client = new Client(_listener.AcceptTcpClient());
                _users.Add(client);

                //soobshit vsem pro soedineniye
                BroadcastConnection();
            }
            static void BroadcastConnection()
            {
                foreach (var user1 in _users)
                {
                    foreach (var user2 in _users)
                    {
                        var broadcastPacket = new PacketBuilder();
                        broadcastPacket.WriteOpCode(1);
                        broadcastPacket.WriteMessage(user2.UserName);
                        broadcastPacket.WriteMessage(user2.UID.ToString());
                        user1.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                    }
                }

            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(5);
                broadcastPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

        }
        public static void BroadcastDisconnectMessage(string uid)
        {
            var disconnectedUser = _users.FirstOrDefault(u => u.UID.ToString() == uid);
            _users.Remove(disconnectedUser);
            foreach (var user in _users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }
            Random rnd = new Random();
            BroadcastMessage($"{disconnectedUser.UserName} has been taken to the dark alley and shot in the back {rnd.Next(0,100)} times");
        }

    }
}