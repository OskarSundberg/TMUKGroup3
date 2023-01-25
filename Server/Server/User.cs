using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        private string name;
        private string ipAddress;
        private bool isOnline;

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        public User(string ipAddress, string name)
        {
            this.ipAddress = ipAddress;
            this.name = name;
        }
    }
}
