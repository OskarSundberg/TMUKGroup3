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
        public static void Main(string[] args)
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
            IPHostEntry? iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            GetIPAddress = iPHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint? iPEndPoint = new IPEndPoint(GetIPAddress, 13375);
            IPEndPoint iPEndPointData = new IPEndPoint(GetIPAddress, 31337);
            Console.WriteLine("Please connect to IP: {0} and port: {1}", GetIPAddress, 13375);
            try
            {
                Socket listener = StartSocket(iPEndPoint);
                Socket listenerData = StartSocket(iPEndPointData);
                while (true)
                {
                    CreateUser(listener, listenerData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private static void CreateUser(Socket listener, Socket listenerData)
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
        private static Socket StartSocket(IPEndPoint? iPEndPoint)
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(iPEndPoint);
            listener.Listen(100);
            return listener;
        }
    }
}