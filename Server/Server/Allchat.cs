using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Allchat
    {
        List<User> userList = new List<User>();
        object lockThread = new object();
        public void UserJoin(User newUser)
        {
            userList.Add(newUser);
            Thread thread = new Thread(() => Monitor(newUser));
            thread.IsBackground = true;
            thread.Start();
            string welcomeMsg = $"Welcome to the chat {newUser.Name}!";
            byte[] echoWelcomeMsg = Encoding.UTF8.GetBytes(welcomeMsg);
            newUser.Handler.Send(echoWelcomeMsg);

        }
        public void Monitor(User user)
        {
            byte[] bytes = new byte[60000];
            int bytesRead;
            string msg = null;

            while (true)
            {
                while (true)
                {
                    bytes = new byte[60000];
                    bytesRead = user.Handler.Receive(bytes);
                    msg += Encoding.UTF8.GetString(bytes);
                    if (msg.IndexOf("\n") > -1)
                    {
                        break;
                    }
                }
                //Testing purpose
                Console.WriteLine($"Text received : {msg}");
                Echo(msg);
                bytes = null;
                msg = null;
                bytesRead = 0;
                Thread.Sleep(1000);
            }

        }
        public int Echo(string msg)
        {
            lock (lockThread)
            {
                byte[] echo = Encoding.UTF8.GetBytes(msg);
                foreach (User u in userList)
                {
                    u.Handler.Send(echo);
                }
                return 1;
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
    }
}
