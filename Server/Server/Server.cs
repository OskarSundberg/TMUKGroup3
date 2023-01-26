using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Server
    {
        public static Allchat allchat = new Allchat();
        static void Main(string[] args)
        {
            StartServer();
            return;
        }
        public static void StartServer()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(hostName);
            IPAddress iPAddress = iPHostEntry.AddressList[1];
            int port = 13375;
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
            Console.WriteLine("Pleas connect to IP: {0} and port: {1}", iPAddress, port);

            try
            {
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(iPEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting for anything");
                    Socket handler = listener.Accept();
                    User user = new User("Name", handler);
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