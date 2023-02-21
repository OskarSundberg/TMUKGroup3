using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace ClientNUnitTest
{
    public class Test
    {
        Client clientTest;
        ConnectionInfo connectionInfoTest;
        Socket listener;
        Socket senderOne;
        ClientPresentation.MainWindow MainWindow;
        [OneTimeSetUp]
        public void Setup()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint server = new IPEndPoint(ipAddress, 13375);

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(server);
            listener.Listen(10);

            clientTest = new Client();
            connectionInfoTest = new ConnectionInfo(IPAddress.Parse("127.0.0.1"), 13375, "Test");

            var t = new Thread(() =>
            {
                MainWindow = new ClientPresentation.MainWindow("test");
                System.Windows.Threading.Dispatcher.Run();
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

        }

        [Test]
        public void TestClientNameAndMsg()
        {
            //ACT
            Client.Sender = senderOne;
            clientTest.Name = "Test";
            clientTest.SendMsg = "Hejsan";

            //ASSERT
            Assert.That(clientTest.Name, Is.EqualTo("Test"));
            Assert.That(clientTest.SendMsg, Is.EqualTo("Hejsan"));
        }

        [Test]
        public void ConnectionInfo_Test()
        {
            Assert.IsNotEmpty(connectionInfoTest.UserName);
            Assert.IsNotNull(connectionInfoTest.IP);
            Assert.IsNotNull(connectionInfoTest.Port);
        }

        // Does not test if connection is established
        [Test]
        public void StartClient_Test()
        {
            void ServerMessage(string message) { }

            Assert.DoesNotThrow(() => clientTest.StartClient(connectionInfoTest, ServerMessage));
        }
        [Test]
        public void GetMessageFromClient_Test()
        {
            Client.Sender = senderOne;
            Assert.DoesNotThrow(() => clientTest.GetMessageFromClient("123123"));
        }

        [Test]
        public void SpamFilter_Test()
        {
            MainWindow.Time = DateTime.Now;

            Thread.Sleep(1000);
            Assert.That(MainWindow.SpamFilter() != false);

            MainWindow.Time = DateTime.Now;
            Assert.That(MainWindow.SpamFilter() == false);
        }
    }
}