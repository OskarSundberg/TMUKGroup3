using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;

namespace ClientBusiness.Model
{
    internal class MessageHandler
    {
        public byte[] SerializeMsg(MsgPacket.Message msg)
        {
            BinaryFormatter? binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
        public MsgPacket.Message DeserializeMsg(byte[] bytes)
        {
            BinaryFormatter? binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                MsgPacket.Message obj = (MsgPacket.Message)binaryFormatter.Deserialize(stream);
                return obj;
            }
        }
    }
}
