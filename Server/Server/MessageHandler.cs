using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    internal class MessageHandler
    {
        public byte[] SerializeMsg(MsgPacket.Message msg)
        {
            string json = JsonSerializer.Serialize(msg);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(json);
            return buffer;
        }
        public MsgPacket.Message DeserializeMsg(byte[] buffer, int bytesReceived)
        {
            string json = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            MsgPacket.Message ?msg = JsonSerializer.Deserialize<MsgPacket.Message>(json)!;
            return msg;
        }
    }
}
