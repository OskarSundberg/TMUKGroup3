using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using NUnit.Framework.Internal;

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
        Socket senderOne;
        Socket senderTwo;
        Socket senderEndOne;
        Socket senderEndTwo;
        Socket listener;

        [OneTimeSetUp]
        public void Setup()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint server = new IPEndPoint(ipAddress, 13375);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(server);
            listener.Listen(10);

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
            testUserTwo = new User("Samme", senderTwo);

            //Creating test user that WILL be closed only use 1 time
            testUserEndOne = new User("Gurra", senderEndOne);
            testUserEndTwo = new User("Ivo", senderEndTwo);

            privateChatTest = new Private_Session(testUserOne, testUserTwo);
        }

        //AllChat Tests

        [Test]
        public void Echo_Testing()
        {
            Assert.That(allChatTest.Echo("123"), Is.EqualTo(1));
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
            Assert.That(privateChatTest.Echo("123"), Is.EqualTo(1));
        }

        //User Tests

        [Test]
        public void User_Test()
        {
            Assert.IsNotNull(testUserOne.Handler);
            Assert.That(testUserOne.Name, Is.EqualTo("Sam"));
            Assert.That(testUserOne.IsOnline, Is.EqualTo(false));
        }

    }
}