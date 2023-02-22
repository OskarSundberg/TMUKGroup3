using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Private_Session
    {
        List<User> members = new List<User>();
        MessageHandler msgHandler = new MessageHandler();
        public Private_Session(User userOne, User userTwo)
        {
            members.Add(userOne);
            members.Add(userTwo);
            foreach (User user in members)
            {
                user.Thread = new Thread(() => Monitor(user));
            }
        }
        public int EndSession(User person)
        {
            try
            {
                person.Handler.Shutdown(SocketShutdown.Both);
                person.Handler.Close();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }
        public int Echo(MsgPacket.Message msg)
        {
            Emoji emoji = new Emoji();
            msg.Msg = emoji.ReplaceEmoji(msg.Msg);
            byte[] echo = msgHandler.SerializeMsg(msg);
            foreach (User u in members)
            {
                u.Handler.Send(echo);
            }
            return 1;
        }
        public void Monitor(User u)
        {
            byte[] bytes = new byte[64000];
            int byteRec;

            while (true)
            {
                bytes = new byte[64000];
                byteRec = u.Handler.Receive(bytes);
                MsgPacket.Message msg = msgHandler.DeserializeMsg(bytes);
                Echo(msg);
                bytes = null;
                msg = null;
                byteRec = 0;
            }
        }
    }
}
