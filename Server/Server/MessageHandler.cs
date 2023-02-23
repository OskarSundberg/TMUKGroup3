using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class MessageHandler
    {
        public byte[] SerializeMsg(MsgPacket.Message msg)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(msg);
            return jsonBytes;
        }
        public MsgPacket.Message DeserializeMsg(byte[] bytes)
        {
            var readOnlySpan = new ReadOnlySpan<byte>(bytes);
            MsgPacket.Message? msg = JsonSerializer.Deserialize<MsgPacket.Message>(readOnlySpan)!;
            return msg;
        }
    }
}
