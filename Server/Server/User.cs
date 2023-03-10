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
        private Socket dataHandler;
        private string name;
        private Thread thread;

        public Socket Handler
        {
            get { return handler; }
            set { handler = value; }
        }

        public Socket DataHandler
        {
            get { return dataHandler; }
            set { dataHandler = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Thread Thread
        {
            get { return thread; }
            set { thread = value; }
        }

        public User(string name, Socket handler)
        {
            this.Name = name;
            this.handler = handler;
        }
    }
}
