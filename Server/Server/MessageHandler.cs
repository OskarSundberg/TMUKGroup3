using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class MessageHandler
    {
        public byte[] SerializeMsg(MsgPacket.Message msg)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
        public MsgPacket.Message DeserializeMsg(byte[] bytes)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                stream.Write(bytes);
                Object obj = binaryFormatter.Deserialize(stream);
                return (MsgPacket.Message)obj;
            }
        }
    }
}
