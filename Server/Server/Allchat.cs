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
        public int UserJoin(User newUser)
        {
            userList.Add(newUser);
            newUser.Thread = new Thread(() => Monitor(newUser));
            newUser.Thread.IsBackground = true;
            newUser.Thread.Start();
            string welcomeMsg = $"Welcome to the chat {newUser.Name}!";
            byte[] echoWelcomeMsg = Encoding.UTF8.GetBytes(welcomeMsg);
            newUser.Handler.Send(echoWelcomeMsg);
            return 1;
        }
        /// <summary>
        /// This function monitors the port for new messages from users.
        /// And uses echo to send out everthing it finds
        /// </summary>
        /// <param name="user"></param>
        public void Monitor(User user)
        {
            byte[] bytes = new byte[64000];
            int bytesRead;
            string msg = null;
            while (true)
            {
                while (true)
                {
                    try
                    {
                        //Waiting for a message then makes it a string and checks if it a valied message 
                        bytes = new byte[64000];
                        bytesRead = user.Handler.Receive(bytes);
                        msg += Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        //Check if '\u009F' is part of a string to check that its a real message
                        if (msg.IndexOf(char.ToString('\u009F')) > -1)
                        {
                            msg = msg.Remove(msg.Length - 1, 1);
                            break;
                        }
                    }
                    //Check if a user have left and then ends the connection to user
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(user.Name);
                        EndSession(user);
                        return;
                    }
                }
                //Testing purpose
                Console.WriteLine($"{msg}");
                Echo(msg);
                bytes = null;
                msg = null;
                bytesRead = 0;
                Thread.Sleep(1000);
            }
        }
        public int Echo(string msg)
        {
            Emoji emoji = new Emoji();
            lock (lockThread)
            {
                msg = emoji.ReplaceEmoji(msg);
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
                userList.Remove(person);
                Echo($"{person.Name} has left the chat");
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
