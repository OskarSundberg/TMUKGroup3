using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;

namespace ClientBusiness.Model
{
    public class MessageHandler
    {
        public byte[] SerializeMsg(MsgPacket.Message msg)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(msg);
            return jsonBytes;
        }
        public MsgPacket.Message DeserializeMsg(byte[] buffer, int bytesReceived)
        {
            string json = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            MsgPacket.Message? msg = JsonSerializer.Deserialize<MsgPacket.Message>(json)!;
            return msg;
        }
    }
}
