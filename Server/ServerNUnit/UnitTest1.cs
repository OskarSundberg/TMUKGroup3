using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;

namespace ServerNUnit
{
    public class Tests
    {
        Allchat echoTest;
        User testUserOne;
        User testUserTwo;

        [SetUp]
        public void Setup()
        {
            echoTest = new Allchat();
            testUserOne = new User("Sam", null);
            testUserTwo = new User("Samme", null);
        }

        [Test]
        public void Echo_Testing()
        {
            //ASSERT
            Assert.AreEqual(1, echoTest.Echo("123"));
        }

        [Test]
        public void User_Test()
        {
            Assert.IsNull(testUserTwo.Handler);
            Assert.AreEqual("Sam", testUserOne.Name);
            Assert.AreEqual(false, testUserOne.IsOnline);
        }
    }
}