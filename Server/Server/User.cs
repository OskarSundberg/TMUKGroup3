using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        private Socket handler;
        private string ipAddress;
        private bool isOnline;

        public Socket Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public bool IsOnline
        {
            get { return isOnline; }
            set { isOnline = value; }
        }

        public User(string ipAddress, Socket handler)
        {
            this.ipAddress = ipAddress;
            this.handler = handler;
        }
    }
}
