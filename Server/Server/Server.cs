using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Server
    {
        static void Main(string[] args)
        {
            StartServer();
            return;
        }
        public static void StartServer()
        {
            IPHostEntry iPHostEntry = Dns.GetHostEntry("localhost");
            IPAddress iPAddress = iPHostEntry.AddressList[1];
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 13375);

            try
            {
                Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(iPEndPoint);

                listener.Listen(10);

                Console.WriteLine("Waiting for anything");
                Socket handler = listener.Accept();

                string msg = null;
                byte[] bytes = null;
                while (true)
                {
                    bytes = new byte[4096];
                    int bytesRead = handler.Receive(bytes);
                    msg += Encoding.UTF8.GetString(bytes);
                    if (msg.IndexOf("\n") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine($"Text received : {msg}");

                byte[] echo = Encoding.UTF8.GetBytes(msg);
                handler.Send(echo);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}