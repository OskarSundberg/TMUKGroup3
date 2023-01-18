using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Sessions
    {
        public static void ServerSession(Socket handler)
        {
            string msg = null;
            byte[] bytes = null;
            while (true)
            {
                bytes = new byte[4096];
                int bytesRead = handler.Receive(bytes);
                msg += Encoding.UTF8.GetString(bytes);
                if (msg.IndexOf("\n") > -1)
                {
                    break;
                }
            }
            Console.WriteLine($"Text received : {msg}");

            byte[] echo = Encoding.UTF8.GetBytes(msg);
            handler.Send(echo);
            handler.Shutdown(SocketShutdown.Both);
            //handler.Close();
        }
    }
}
