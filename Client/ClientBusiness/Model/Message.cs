using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBusiness.Model
{
    internal class Message
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
        public byte[] SerializeMsg(Message msg)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
        public Object DeserializeMsg(byte[] bytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes);
                Object obj = binaryFormatter.Deserialize(stream);
                return obj;
            }
        }
    }
}
