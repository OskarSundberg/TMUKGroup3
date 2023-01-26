using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;
using System.Net;
using System.Reflection;

namespace ServerNUnit
{
    public class Tests
    {
        Allchat allChatTest;
        User testUserOne;
        User testUserTwo;
        Socket senderOne;
        Socket senderTwo;
        Socket listener;

        [SetUp]
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

            allChatTest = new Allchat();
            testUserOne = new User("Sam", senderOne);
            testUserTwo = new User("Samme", senderTwo);
        }

        [Test]
        public void Echo_Testing()
        {
            //ASSERT
            Assert.AreEqual(1, allChatTest.Echo("123"));
        }

        [Test]
        public void User_Test()
        {
            Assert.IsNotNull(testUserOne.Handler);
            Assert.AreEqual("Sam", testUserOne.Name);
            Assert.AreEqual(false, testUserOne.IsOnline);
        }
        [Test]
        public void UserJoin_Test()
        {
            try
            {
                allChatTest.UserJoin(testUserOne);
            }
            catch(Exception ex) 
            { 
                Assert.Fail(ex.Message);
            }
        }    
    }
}