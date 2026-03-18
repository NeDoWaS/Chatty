using ChattyClient.MVM.Core;
using ChattyClient.MVM.Model;
using ChattyClient.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ChattyClient.MVM.ViewModel
{
    public class MainViewModel
    {
        public ObservableCollection<UserModel> Users { get; set; }

        public ObservableCollection<string> Messages { get; set; }
        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        private Server _server;
        public string Username { get; set; }
        public string Message { get; set; }
        public MainViewModel()
        {
            Users = new ObservableCollection<UserModel>();
            Messages = new ObservableCollection<string>();

            _server = new Server();

            _server.connectedEvent += UserConnected;
            _server.msgRecievedEvent += MessageRecueved;
            _server.usrDisconnectedEvent += UserDisconnected;

            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrEmpty(Username));

            SendMessageCommand = new RelayCommand(o => _server.SendMessageToServer(Message), o => !string.IsNullOrEmpty(Message));
        }




        private void UserConnected()
        {
            var user = new UserModel
            {
                Username = _server.PacketReader.ReadString(),
                UID = _server.PacketReader.ReadString()
            };

            if (!Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }

        private void UserDisconnected()
        {
            var uid = _server.PacketReader.ReadString();
            var user = Users.FirstOrDefault(x => x.UID == uid);
        }

        private void MessageRecueved()
        {
            var msg = _server.PacketReader.ReadString();
            Application.Current.Dispatcher.Invoke(() => Messages.Add(msg));
        }
    }
}
