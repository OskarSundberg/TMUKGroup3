using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBusiness.Model
{
    public class Message
    {
        private string msg;
        private string userFrom;

        public string UserTo { get; private set; }
        public string UserFrom { get { return userFrom; } }
        public string Msg { get { return msg; } }
        public Message(string msg, string userFrom)
        {
            this.msg = msg;
            this.userFrom = userFrom;
        }
    }
}
