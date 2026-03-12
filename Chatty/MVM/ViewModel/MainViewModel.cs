using ChattyClient.MVM.Core;
using ChattyClient.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChattyClient.MVM.ViewModel
{
    class MainViewModel
    {
        public RelayCommand ConnectToServerCommand { get; set; }
        private Server _server;
        public string Username { get; set; }
        public MainViewModel()
        {
            _server = new Server();
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o=> !string.IsNullOrEmpty(Username));
        }
    }
}
