using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using NUnit.Framework.Internal;
using System.Text;

namespace ServerNUnit
{
    public class Tests
    {
        Allchat allChatTest;
        Private_Session privateChatTest;
        User testUserOne;
        User testUserEndOne;
        User testUserTwo;
        User testUserEndTwo;
        MsgPacket.Message testMessageOne;
        Socket senderOne;
        Socket senderTwo;
        Socket senderEndOne;
        Socket senderEndTwo;
        Socket listener;
        Socket dataListener;
        Socket dataOne;

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

            allChatTest = new Allchat();

            //Creating test user that will not be closed
            testUserOne = new User("Sam", senderOne);
            testUserOne.DataHandler = dataOne;
            testUserTwo = new User("Samme", senderTwo);

            //Creating test user that WILL be closed only use 1 time
            testUserEndOne = new User("Gurra", senderEndOne);
            testUserEndTwo = new User("Ivo", senderEndTwo);

            privateChatTest = new Private_Session(testUserOne, testUserTwo);
            testMessageOne = new MsgPacket.Message("123", testUserOne.Name);
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

        //Private_Session Tests

        [Test]
        public void StartPrivateChat_Test()
        {
            Private_Session test = new Private_Session(testUserOne, testUserTwo);
            Assert.IsTrue(test.GetType() == typeof(Private_Session));
        }

        [Test]
        public void EndSessionPrivate_Test()
        {
            Assert.That(privateChatTest.EndSession(testUserEndTwo), Is.EqualTo(1));
        }

        [Test]
        public void EchoPrivate_Testing()
        {
            MsgPacket.Message msg = new("123", testUserOne.Name);
            Assert.That(privateChatTest.Echo(msg), Is.EqualTo(1));
        }

        //User Tests

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
                for (int i = 0; i < 101; i++)
                {
                    Socket client = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    client.Connect(server);
                    byte[] cUseName = Encoding.UTF8.GetBytes($"{i}");
                    client.Send(cUseName);
                    Socket dataport = new Socket(Server.Server.GetIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    dataport.Connect(dataServer);
                }
            });
        }
    }
}