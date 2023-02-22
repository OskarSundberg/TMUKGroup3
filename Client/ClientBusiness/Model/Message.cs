using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBusiness.Model
{
    [Serializable]
    public class Message
    {
        public string UserTo { get; set; }
        public string UserFrom { get; set; }
        public string Msg { get; set; }
        public Message(string msg, string userFrom)
        {
            this.Msg = msg;
            this.UserFrom = userFrom;
        }
    }
}
