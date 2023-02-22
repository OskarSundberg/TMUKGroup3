using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MsgPacket
{
    [Serializable]
    public class Message : SerializationBinder
    {
        public string UserTo { get; set; }
        public string UserFrom { get; set; }
        public string Msg { get; set; }
        public Message(string msg, string userFrom)
        {
            this.Msg = msg;
            this.UserFrom = userFrom;
        }
        public override Type BindToType(string AssemblyName, string TypeName)
        {
            Type typeToDeserialize = Type.GetType(String.Format(" {0}, {1}", TypeName, AssemblyName));
            return typeToDeserialize;
        }
    }
}
