using NUnit.Framework;
using System.Net.WebSockets;
using Server;
using System.Net.Sockets;

namespace ServerNUnit
{
    public class Tests
    {
        [Test]
        public void Echo_Testing()
        {
            //ARANGE
            var echoTest = new Allchat();

            //ACT
            echoTest.Echo("");

            //ASSERT
            Assert.AreEqual(1, echoTest.Echo(""));
        }

        [Test]
        public void User_Test()
        {
            var user = new User("Sam", null);

            user.Name = "Test";
            user.IsOnline = false;

            Assert.AreEqual("Test", user.Name);
            Assert.AreEqual(false, user.IsOnline);
        }
    }
}