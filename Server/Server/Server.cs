using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    internal class Server
    {
        public static IPAddress? GetIPAddress { get; set; }
        public static Allchat allchat = new Allchat();
        static void Main(string[] args)
        {
            StartServer();
            return;
        }

        /// <summary>
        /// This method starts up the server and uses a try catch block to make sure everything works as it should.
        /// </summary>
        /// <param></param>
        public static void StartServer()
        {
            string? hostName = Dns.GetHostName();
            IPHostEntry? iPHostEntry = Dns.GetHostEntry(hostName);
            IPAddress? iPAddress = iPHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            GetIPAddress = iPAddress;
            int port = 13375;
            IPEndPoint? iPEndPoint = new IPEndPoint(iPAddress, port);
            int dataPort = 31337;
            IPEndPoint iPEndPointData = new IPEndPoint(iPAddress, dataPort);
            Console.WriteLine("Please connect to IP: {0} and port: {1}", iPAddress, port);

            try
            {
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket listenerData = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(iPEndPoint);
                listener.Listen(100);
                listenerData.Bind(iPEndPointData);
                listenerData.Listen(100);

                while (true)
                {
                    Console.WriteLine("Waiting for anything");
                    Socket handler = listener.Accept();
                    byte[] bytes = new byte[64000];
                    int bytesRead = handler.Receive(bytes);
                    string name = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                    User user = new User(name, handler);
                    Console.WriteLine("Waiting for connection to data port");
                    Socket dataHandler = listenerData.Accept();
                    user.DataHandler = dataHandler;
                    allchat.UserJoin(user);
                    Console.WriteLine("Sent connection to session");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}