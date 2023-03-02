using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Server
{
    internal class ServerStartUp
    {
        public Socket? ServerSocket { get { return serverSocket; } }
        public Socket? ServerSocketData { get { return serverSocketData; } }
        public IPAddress? GetIPAddress { get; set; }
        public Allchat allchat = new Allchat();

        private Socket? serverSocketData;
        private Socket? serverSocket;
        
        /// <summary>
        /// This method starts up the server and uses a try catch block to make sure everything works as it should.
        /// </summary>
        /// <param></param>
        public void StartServer()
        {
            IPHostEntry? iPHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            GetIPAddress = iPHostEntry.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint? iPEndPoint = new IPEndPoint(GetIPAddress, 13375);
            IPEndPoint iPEndPointData = new IPEndPoint(GetIPAddress, 31337);
            Console.WriteLine($"Please connect to IP: {GetIPAddress} and port: {13375}");
            try
            {
                serverSocket = StartSocket(iPEndPoint);
                serverSocketData = StartSocket(iPEndPointData);
                while (true)
                {
                    CreateUser(serverSocket, serverSocketData);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void CreateUser(Socket listener, Socket listenerData)
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
        private Socket StartSocket(IPEndPoint? iPEndPoint)
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(iPEndPoint);
            listener.Listen(100);
            return listener;
        }
        public void CloseServer()
        {
            ServerSocket.Close();
            ServerSocketData.Close();
        }
    }
}