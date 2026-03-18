using ChattyServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNetCore.SignalR;


namespace ChattyServer
{
    public class Client
    {
        public string UserName { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }

        PacketReader _packetReader;

        public class ChatHub : Hub
        {
            public async Task JoinChat(string username)
            {
                await Clients.All.SendAsync("UserJoined", username);
            }

            public async Task SendMessage(string username, string message)
            {
                await Clients.All.SendAsync("ReceiveMessage", username, message);
            }
        }
    }
}
