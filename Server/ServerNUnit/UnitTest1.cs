using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using NUnit.Framework.Internal;
using System.Text;
using System;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;


namespace ServerNUnit
{
    public class Tests
    {
        Allchat allChatTest;
        User testUserOne;
        User testUserEndOne;
        User testUserTwo;
        User testUserEndTwo;
        User testUser1, testUser2, testUser3, testUser4;
        MsgPacket.Message testMessageOne;
        Socket senderOne;
        Socket senderTwo;
        Socket senderEndOne;
        Socket senderEndTwo;
        Socket listener;
        Socket dataListener;
        Socket dataOne;
        Socket testUser1Socket, testUser2Socket, testUser3Socket, testUser4Socket;
        Emoji emoji = new Emoji();

        [OneTimeSetUp]
        public void Setup()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint server = new IPEndPoint(ipAddress, 13375);
            IPEndPoint serverData = new IPEndPoint(ipAddress, 31337);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(server);
            listener.Listen(10);

            dataListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            dataListener.Bind(serverData);
            dataListener.Listen(10);

            dataOne = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            dataOne.Connect(serverData);

            senderOne = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderOne.Connect(server);

            senderTwo = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderTwo.Connect(server);

            senderEndOne = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderEndOne.Connect(server);

            senderEndTwo = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            senderEndTwo.Connect(server);

            testUser1Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            testUser1Socket.Connect(server);

            testUser2Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            testUser2Socket.Connect(server);

            testUser3Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            testUser3Socket.Connect(server);

            testUser4Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            testUser4Socket.Connect(server);

            allChatTest = new Allchat();

            //Creating test user that will not be closed
            testUserOne = new User("Sam", senderOne);
            testUserOne.DataHandler = dataOne;
            testUserTwo = new User("Samme", senderTwo);

            //Creating test user that WILL be closed only use 1 time
            testUserEndOne = new User("Gurra", senderEndOne);
            testUserEndTwo = new User("Ivo", senderEndTwo);

            testMessageOne = new MsgPacket.Message("123", testUserOne.Name);

            testUser1 = new User("MrGustavo", testUser1Socket);
            testUser1.DataHandler = dataOne;
            testUser2 = new User("MrBigGuy", testUser2Socket);
            testUser2.DataHandler = dataOne;
            testUser3 = new User("EdSheeran", testUser3Socket);
            testUser3.DataHandler = dataOne;
            testUser4 = new User("MyLegsDontWork", testUser4Socket);
            testUser4.DataHandler = dataOne;


        }

        //AllChat Tests

        [Test]
        public void Echo_Testing()
        {
            Assert.That(allChatTest.Echo(testMessageOne), Is.EqualTo(1));
        }

        [Test]
        public void UserJoin_Test()
        {
            Assert.That(allChatTest.UserJoin(testUserOne), Is.EqualTo(1));
        }

        [Test]
        public void EndSession_Test()
        {
            Assert.That(allChatTest.EndSession(testUserEndOne), Is.EqualTo(1));
        }

        [Test]
        public void SendUsersOnlineList_Test()
        {
            allChatTest.UserJoin(testUserOne);
            Assert.DoesNotThrow(() => { allChatTest.SendUsersOnlineList(); });
        }

        [Test]
        public void Invalid_Message_Test()
        {
            MsgPacket.Message msg = new("MessageOne", "UserOne");
            MessageHandler msgHandler = new MessageHandler();
            byte[] msgByte = msgHandler.SerializeMsg(msg);
            int bytesRecived = msgByte.Length;
            MsgPacket.Message recivedMsg = msgHandler.DeserializeMsg(msgByte, bytesRecived);
            Assert.That(recivedMsg.UserFrom, Is.EqualTo(msg.UserFrom));
            Assert.That(recivedMsg.Msg, !Is.EqualTo("SomethingDifferent"));
        }

        //User Tests

        [Test]
        public void Users_In_Server_Test()
        {
            Allchat testchat = new Allchat();
            testchat.UserJoin(testUser1);
            testchat.UserJoin(testUser2);
            testchat.UserJoin(testUser3);
            testchat.UserJoin(testUser4);
            Assert.That(testchat.UserList.Count, Is.EqualTo(4));
            Assert.That(testchat.UserList[0].Name, Is.EqualTo("MrGustavo"));
            Assert.That(testchat.UserList[1].Name, Is.EqualTo("MrBigGuy"));
            Assert.That(testchat.UserList[2].Name, Is.EqualTo("EdSheeran"));
            Assert.That(testchat.UserList[3].Name, Is.EqualTo("MyLegsDontWork"));
        }

        [Test]
        public void User_Test()
        {
            Assert.IsNotNull(testUserOne.Handler);
            Assert.That(testUserOne.Name, Is.EqualTo("Sam"));
            Assert.That(testUserOne.IsOnline, Is.EqualTo(false));
        }

        [Test]
        public void Emoji_Test()
        {
            Emoji emoji = new Emoji();
            string test = emoji.emojiDic.Keys.First();
            test = emoji.ReplaceEmoji(test);
            Assert.IsTrue(emoji.emojiDic.ContainsValue(test));
            test = emoji.emojiDic.Keys.Last();
            test = emoji.ReplaceEmoji(test);
            Assert.IsTrue(emoji.emojiDic.ContainsValue(test));
        }

        [Test]
        //a string whit no emoji should not be changed when run through ReplaceEmoji
        public void Emoji_Test_v2()
        {
            string input, output;
            for (int i = 0; i < 100; i++)
            {
                input = "test" + i;
                output = emoji.ReplaceEmoji(input);
                Assert.IsTrue(input.Equals(output));
            }
        }
        [Test]
        public void Server_Overload_Test()
        {
            Thread thread = new Thread(() => Server.Server.Main(null));
            thread.IsBackground = true;
            thread.Start();
            Thread.Sleep(1000);
            Assert.DoesNotThrow(() =>
            {
                IPEndPoint server = new IPEndPoint(Server.Server.GetIPAddress, 13375);
                IPEndPoint dataServer = new IPEndPoint(Server.Server.GetIPAddress, 31337);
                for (int i = 0; i < 100; i++)
                {
                    Socket client = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(server);
                    byte[] cUseName = Encoding.UTF8.GetBytes($"{i}");
                    client.Send(cUseName);
                    Socket dataport = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    dataport.Connect(dataServer);
                }
            });
            Server.Server.ServerSocket.Close();
            Server.Server.ServerSocketData.Close();
        }
        [Test]
        public void OutputLeaveTest()
        {
            string userName = "asdads";
            Thread thread = new Thread(() => Server.Server.StartServer());
            thread.IsBackground = true;
            thread.Start();
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            Thread.Sleep(1000);
            IPEndPoint server = new IPEndPoint(Server.Server.GetIPAddress, 13375);
            IPEndPoint dataServer = new IPEndPoint(Server.Server.GetIPAddress, 31337);
            Socket client = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(server);
            byte[] cUseName = Encoding.UTF8.GetBytes($"{userName}");
            client.Send(cUseName);
            Socket dataport = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            dataport.Connect(dataServer);
            client.Disconnect(true);
            Thread.Sleep(1000);
            string expected = $"{userName} has closed it's connection!";
            Assert.That(stringWriter.ToString().Contains(expected));
            Server.Server.ServerSocket.Close();
            Server.Server.ServerSocketData.Close();
        }

        [Test]
        public void MessageHadler_Test()
        {
            MsgPacket.Message msg = new("ABC123", "Sam");
            MessageHandler msgHandler = new MessageHandler();
            byte[] msgByte = msgHandler.SerializeMsg(msg);
            int bytesRecived = msgByte.Length;
            MsgPacket.Message recivedMsg = msgHandler.DeserializeMsg(msgByte, bytesRecived);
            Assert.That(recivedMsg.UserFrom, Is.EqualTo(msg.UserFrom));
            Assert.That(recivedMsg.Msg, Is.EqualTo(msg.Msg));
        }
    }
}