using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Message
    {
        private string msg;
        private string userFrom;

        public string UserTo { get; private set; }
        public string UserFrom { get {return userFrom; } }
        public string Msg { get { return msg; } }
        public Message(string msg, string userFrom) 
        {
            this.msg = msg;
            this.userFrom = userFrom;
        }
    }
}
