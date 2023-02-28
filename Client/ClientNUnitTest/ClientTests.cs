using ClientPresentation;
using ClientPresentation.Views;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MsgPacket;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Xml.Linq;

namespace ClientNUnitTest
{
    public class Test
    {
        Client clientTest;
        ConnectionInfo connectionInfoTest;
        Socket listener;
        Socket dataListener;
        Socket senderOne;
        RandomGen rg;
        static MainWindow MainWindow { get; set; }
        static EstablishConnection ec { get; set; }
        [OneTimeSetUp]
        public void Setup()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint server = new IPEndPoint(ipAddress, 13375);
            IPEndPoint dataconnection = new IPEndPoint(ipAddress, 31337);
            rg = new RandomGen();

            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            dataListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(server);
            dataListener.Bind(dataconnection);
            listener.Listen(10);
            dataListener.Listen(10);

            clientTest = new Client();
            connectionInfoTest = new ConnectionInfo(ipAddress, 13375, "Test");
            Thread t = new Thread(() =>
            {
                MainWindow = new MainWindow("test");
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            Thread t2 = new Thread(() =>
            {
                ec = new EstablishConnection();
            });
            t2.SetApartmentState(ApartmentState.STA);
            t2.Start();
            t2.Join();
        }

        #region [client]
        [Test]
        public void TestClientNameAndMsg()
        {
            //ACT
            Client.Sender = senderOne;
            clientTest.Name = "Test";
            clientTest.SendMsg = "Hejsan";

            //ASSERT
            //Assert.That(clientTest.Name, Is.EqualTo("Test"));
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
        //[Timeout (2000)]
        public void StartClient_Test()
        {
            void ServerMessage(string message) { }
            void DataMessage(string[] data) { }
            Client.Sender = senderOne;
            Assert.DoesNotThrow(() => clientTest.StartClient(connectionInfoTest, ServerMessage, DataMessage));
        }
        [Test]
        public void GetMessageFromClient_Test()
        {
            Client.Sender = senderOne;
            Assert.DoesNotThrow(() => clientTest.GetMessageFromClient("123123"));
        }
        #endregion

        #region [MainWindo]

        [Test]
        public void Send_message_Test()
        {
            Assert.IsTrue(MainWindow.Send_message("test"));
        }
        /// <summary>
        /// Test that user cant send more the one message a secconed
        /// </summary>
        [Test]
        public void SpamFilter_Test()
        {
            MainWindow.Time = DateTime.Now;
            Thread.Sleep(1000);
            Assert.IsTrue(MainWindow.SpamFilter());

            MainWindow.Time = DateTime.Now;
            Assert.IsFalse(MainWindow.SpamFilter());

        }
        #endregion

        #region [Model]
        /// <summary>
        /// a properti of the method is that two equal strings should give the same byte array. 
        /// the test asserts that the same msg will reslut in the same utput.
        /// </summary>
        [Test]
        public void MessageHandler_Test()
        {
            byte[] a, b;
            string str_message, userName;
            MsgPacket.Message msg;
            userName = "botenAnna";
            RandomGen rg = new RandomGen();
            MessageHandler messageHandler = new MessageHandler();

            for (int i = 0; i < 100; i++)
            {
                str_message = rg.Gen_String(10);
                msg = new MsgPacket.Message(str_message, userName);
                a = messageHandler.SerializeMsg(msg);
                b = messageHandler.SerializeMsg(msg);
                Assert.That(b, Is.EqualTo(a));
            }
        }
        /// <summary>
        /// a properti of the method is that two equal strings should give the same byte array. 
        /// Then the opposite should hold. two different messegs gives 2 different byte arrays
        /// </summary>
        [Test]
        public void MessageHandler_Test_Difrent_Messeges()
        {
            byte[] a, b;
            string str_message, userName;
            MsgPacket.Message msg;
            userName = "botenAnna";
            MessageHandler messageHandler = new MessageHandler();

            str_message = rg.Gen_String(10);
            msg = new MsgPacket.Message(str_message, userName);
            a = messageHandler.SerializeMsg(msg);
            Thread.Sleep(20);
            for (int i = 0; i < 100; i++)
            {
                str_message = rg.Gen_String(10);
                msg = new MsgPacket.Message(str_message, userName);
                b = messageHandler.SerializeMsg(msg);

                Assert.False(a == b);
            }
        }
        #endregion

        #region [RandomGen]
        /// <summary>
        /// If a == b and then b == C we konwe a == C by Transitivity.
        /// the random funktions whit 128 difrent utputs, gives (1/128)^3 whitch is very low. 
        /// so if all a b and c are the same then somthing is wrong.
        /// </summary>
        [Test]
        public void Gen_Int_Test()
        {
            int a, b, c;
            a = rg.Gen_Int();
            b = rg.Gen_Int();
            c = rg.Gen_Int();
            if (a == b)
                Assert.False(b == c);
            else
                Assert.NotNull(a);

        }
        #endregion

        #region [EstablishConnection]
        /// <summary>
        /// Test some basic in EstablishConnection 
        /// </summary>
        [Test]
        public void EstablishConnection_Test()
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            ec.IpAddress = IPAddress.Parse("127.0.0.1");
            Assert.That(ip, Is.EqualTo(ec.IpAddress));

            string name = "Test";
            ec.Name = name;
            Assert.That(name, Is.EqualTo(ec.Name));
            ec.Name = "NotTest";
            Assert.That(name, Is.Not.EqualTo(ec.Name));

            string port = "13375";
            ec.PortNumber = port;
            Assert.That(port, Is.EqualTo(ec.PortNumber));
            ec.PortNumber = "123";
            Assert.That(name, Is.Not.EqualTo(ec.Name));
        }
        #endregion

    }
}