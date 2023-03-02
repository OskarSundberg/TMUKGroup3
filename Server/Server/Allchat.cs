using MsgPacket;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        MessageHandler msgHandler = new MessageHandler();

        public List<User> UserList { get { return userList; } }

        /// <summary>
        /// This method adds a User to the userList and starts a thread for the User.
        /// The method also displays a welcome message for the User.
        /// </summary>
        /// <param name="newUser"></param>
        public int UserJoin(User newUser)
        {
            userList.Add(newUser);
            newUser.Thread = new Thread(() => Monitor(newUser));
            newUser.Thread.IsBackground = true;
            newUser.Thread.Start();
            string welcomeMsg = $"Welcome to the chat {newUser.Name}!";
            MsgPacket.Message welcomeMessage = new(welcomeMsg, "Server");
            newUser.Handler.Send(msgHandler.SerializeMsg(welcomeMessage));
            SendUsersOnlineList();
            return 1;
        }

        /// <summary>
        /// This method monitors the port for new messages from users.
        /// And uses echo to send out everthing it finds
        /// </summary>
        /// <param name="user"></param>
        public void Monitor(User user)
        {
            byte[]? bytes = new byte[64000];
            int bytesRead;
            while (true)
            {
                try
                {
                    //Waiting for a message then makes it a string and checks if it a valied message 
                    bytes = new byte[64000];
                    bytesRead = user.Handler.Receive(bytes);
                    MsgPacket.Message? message = msgHandler.DeserializeMsg(bytes, bytesRead);
                    //Testing purpose
                    Console.WriteLine($"{message.Msg}");
                    Whisper(message, user);
                    bytes = null;
                    message = null;
                    bytesRead = 0;
                    Thread.Sleep(1000);

                }
                //Check if a user have left and then ends the connection to user
                catch
                {
                    Console.Write($"{user.Name} has closed it's connection!");
                    EndSession(user);
                    return;
                }
            }
        }
        public void MessageChecks(MsgPacket.Message message, User user)
        {
            switch (message.Msg)
            {
                case "/online":
                    string usersOnline = "Users curently online:";
                    foreach (User u in userList)
                    {
                        usersOnline += "\n" + u.Name;
                    }
                    MsgPacket.Message realmsg = new MsgPacket.Message(usersOnline, "Server");
                    user.Handler.Send(msgHandler.SerializeMsg(realmsg));
                    break;
                case "/emoji":
                    user.Handler.Send(msgHandler.SerializeMsg(SendEmojiList()));
                    break;
                default:
                    Echo(message);
                    break;
            }
        }
        public void Whisper(MsgPacket.Message message, User userFrom)
        {
            Emoji emoji = new Emoji();
            if (message.UserTo != null)
            {
                message.Msg = emoji.ReplaceEmoji(message.Msg);
                User? userTo = userList.Find(_ => _.Name == message.UserTo);
                if (userTo != null)
                {
                    userTo.Handler.Send(msgHandler.SerializeMsg(message));
                    userFrom.Handler.Send(msgHandler.SerializeMsg(message));
                }
                else
                {
                    MsgPacket.Message errorMessage = new MsgPacket.Message($"Can't find user {message.UserTo}", "Server");
                    userFrom.Handler.Send(msgHandler.SerializeMsg(errorMessage));
                }
            }
            else
            {
                MessageChecks(message, userFrom);
            }
        }

        /// <summary>
        /// This method echoes the message from one user to all other users.
        /// </summary>
        /// <param name="msg"></param>
        public int Echo(MsgPacket.Message msg)
        {
            Emoji emoji = new Emoji();
            lock (lockThread)
            {
                msg.Msg = emoji.ReplaceEmoji(msg.Msg);
                byte[] echo = msgHandler.SerializeMsg(msg);
                foreach (User u in userList)
                {
                    u.Handler.Send(echo);
                }
                return 1;
            }
        }
        public void SendUsersOnlineList()
        {
            string? usersOnline = null;
            foreach (User u in userList)
            {
                usersOnline += u.Name + ",";
            }
            if (usersOnline != null)
            {
                usersOnline = usersOnline.Remove(usersOnline.Length - 1);
                try
                {
                    _ = EchoData(usersOnline);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public Message SendEmojiList()
        {
            Emoji emoji = new Emoji();
            string? emojiList = null;
            List<string> emojiStr;
            emojiStr = emoji.emojiDic.Keys.ToList();
            foreach (string e in emojiStr)
            {
                emojiList += e + "->" + emoji.ReplaceEmoji(e) + "\n";
            }

            return new Message(emojiList, "Server");
        }
        public int EchoData(string msg)
        {
            lock (lockThread)
            {
                byte[] echo = Encoding.UTF8.GetBytes(msg);
                foreach (User u in userList)
                {
                    u.DataHandler.Send(echo);
                }
                return 1;
            }
        }

        /// <summary>
        /// This method ends the session between a user and server.
        /// </summary>
        /// <param name="user"></param>
        public int EndSession(User user)
        {
            try
            {
                user.Handler.Shutdown(SocketShutdown.Both);
                user.Handler.Close();
                user.DataHandler.Shutdown(SocketShutdown.Both);
                user.DataHandler.Close();
                userList.Remove(user);
                string userLeave = $"{user.Name} has left the chat";
                MsgPacket.Message msg = new(userLeave, user.Name);
                Echo(msg);
                SendUsersOnlineList();
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
