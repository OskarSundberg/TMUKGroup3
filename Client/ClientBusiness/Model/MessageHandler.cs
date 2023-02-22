using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientBusiness.Model
{
    internal class MessageHandler
    {
        public byte[] SerializeMsg(Message msg)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
        public Message DeserializeMsg(byte[] bytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes);
                Object obj = binaryFormatter.Deserialize(stream);
                return (Message)obj;
            }
        }
    }
}
