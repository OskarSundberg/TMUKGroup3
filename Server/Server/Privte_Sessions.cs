using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Privte_Sessions
    {
        List<User> members = new List<User>();
        public Privte_Sessions(User userOne, User userTwo)
        {
            members.Add(userOne);
            members.Add(userTwo);
        }
        private void Start_PrivteChat(User personOne, User personTwo)
        {
            foreach (User user in members)
            {
                Thread thread = new Thread(() => Monitor(user));
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
        public int Echo(string msg)
        {
            byte[] echo = Encoding.UTF8.GetBytes(msg);
            foreach(User u in members)
            {
                u.Handler.Send(echo);
            }
            return 1;
        }
        public void Monitor(User u)
        {
            byte[] bytes = new byte[4096];
            int byteRec;

            while (true)
            {
                byteRec = u.Handler.Receive(bytes);
                Echo(Encoding.UTF8.GetString(bytes, 0, byteRec));
            }
        }
    }
}
