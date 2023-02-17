using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ClientBusiness.Model
{
    public class ConnectionInfo
    {
        public ConnectionInfo(IPAddress IP, int Port, string UserName)
        {
            this.IP = IP;
            this.Port = Port;
            this.UserName = UserName;
        }

        public IPAddress IP { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
    }
}
