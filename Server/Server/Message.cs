using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Message
    {
        private string msg;
        private string userFrom;

        public string UserTo { get; private set; }
        public string UserFrom { get { return userFrom; } }
        public string Msg { get; set; }
        public Message(string msg, string userFrom)
        {
            this.msg = msg;
            this.userFrom = userFrom;
        }
    }
}
