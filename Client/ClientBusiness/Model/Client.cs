namespace ClientBusiness.Model
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string name;

        public Socket sender { get; private set; }
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }
        public void StartClient()
        {
            

            try
            {
                IPAddress ipAddress = IPAddress.Parse(Console.ReadLine());
                IPEndPoint server = new IPEndPoint(ipAddress, 13375);

                sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(server);

                    Console.WriteLine("Socket connected to {0}", sender.RemoteEndPoint.ToString());

                    //this line gets userinput from console and sends to server, could be used later on in thread
                    //Thread sendMessageThread = new Thread(() => GetMessageFromClient());
                    Thread sendMessageThread = new Thread(GetMessageFromClient);
                    sendMessageThread.IsBackground = true;
                    sendMessageThread.Start();

                    //int bytesSent = sender.Send(msg);
                    //Thread recieveMessageThread = new Thread(() => RecieveMessageFromServer());
                    Thread recieveMessageThread = new Thread(RecieveMessageFromServer);
                    recieveMessageThread.IsBackground = true;
                    recieveMessageThread.Start();

                    //sender.Shutdown(SocketShutdown.Both);
                    //sender.Close();
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException : {0}", e.ToString());
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException : {0}", e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void GetMessageFromClient()
        {
            while (true) 
            {
                //test prompt, remove later and only keep the return line
                Console.WriteLine("Enter the text you want to send here: ");
                byte[] msg = Encoding.UTF8.GetBytes(Console.ReadLine() + "\n");

                int bytesSent = sender.Send(msg);
            }
        }

        public void RecieveMessageFromServer()
        {
            int bytesRec;
            byte[] bytes = new byte[4100];
            while (true)
            {
                bytesRec = sender.Receive(bytes);
                if (bytesRec != 0)
                {
                    Console.WriteLine("Echoed test = {0}", Encoding.UTF8.GetString(bytes, 0, bytesRec));
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}