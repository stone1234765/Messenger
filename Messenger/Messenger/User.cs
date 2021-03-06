﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messenger
{
    class User
    {
        public User(Socket socket, string nickname)
        {
            this.Socket = socket;
            this.Nickname = nickname;
            communication = new Communication(socket);
        }
        public object MessagesLock = new object();
        public List<string> UnReadMessages = new List<string>();
        public Socket Socket;
        public string Nickname;
        public Communication communication;
    }
}
