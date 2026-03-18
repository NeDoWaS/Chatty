using ChattyClient.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using static ChattyServer.Client;
using Microsoft.AspNetCore.SignalR;

namespace ChattyServer
{
    class Program ()
    {
        static List<Client> _users;
        static TcpListener _listener;
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MapHub<ChatHub>("/chat");

            app.Run();
        }

    }
}