namespace ClientBusiness.Model
{
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml.Linq;

    public class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string name;

        public static Socket Sender { get; set; }
        public static Socket DataSender { get; set; }

        //callback
        private Action<string> _messageCallBack;

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

        private string sendMsg;
        public string SendMsg
        {
            get { return sendMsg; }
            set
            {
                if (value == null || Name == null)
                    return;

                GetMessageFromClient(Name + ": " + value);
                sendMsg = value;
                OnPropertyChanged();
            }
        }

        private string receivedMsg;
        public string ReceivedMsg
        {
            get { return receivedMsg; }
            set
            {
                if (value == receivedMsg)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }
        public void StartClient(ConnectionInfo cInfo, Action<string> massageCallBack)
        {
            try
            {
                //IPAddress ipAddress = IPAddress.Parse(Ip);
                IPEndPoint server = new IPEndPoint(cInfo.IP, cInfo.Port);

                Sender = new Socket(cInfo.IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._messageCallBack = massageCallBack;

                try
                {
                    Sender.Connect(server);
                    if (!SocketConnected(Sender))
                    {
                        throw new Exception("Not Connected");
                    }
                    byte[] cUseName = Encoding.UTF8.GetBytes(cInfo.UserName);
                    Sender.Send(cUseName);
                    Console.WriteLine("Socket connected to {0}", Sender.RemoteEndPoint.ToString());

                    int bytesRec;
                    byte[] bytes = new byte[64000];
                    bytesRec = Sender.Receive(bytes);
                    string dataPort = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    try
                    {
                        IPEndPoint serverData = new IPEndPoint(cInfo.IP, Int32.Parse(dataPort));
                        DataSender = new Socket(cInfo.IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        DataSender.Connect(serverData);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    Thread recieveDataThread = new Thread(RecieveDataFromServer);
                    recieveDataThread.IsBackground = true;
                    recieveDataThread.Start();

                    Thread recieveMessageThread = new Thread(RecieveMessageFromServer);
                    recieveMessageThread.IsBackground = true;
                    recieveMessageThread.Start();
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

        public void GetMessageFromClient(string msgstr)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(msgstr + char.ToString('\u009F'));
                int bytesSent = Sender.Send(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void RecieveMessageFromServer()
        {
            int bytesRec;
            byte[] bytes = new byte[64000]; //nearly max size
            while (true)
            {
                bytesRec = Sender.Receive(bytes);
                if (bytesRec != 0)
                {
                    this._messageCallBack(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                }
            }
        }

        public void RecieveDataFromServer()
        {
            int bytesRec;
            byte[] bytes = new byte[64000];
            while (true)
            {
                bytesRec = DataSender.Receive(bytes);
                if (bytesRec != 0)
                {
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        bool SocketConnected(Socket socket)
        {
            bool connectionSent = socket.Poll(1000, SelectMode.SelectRead);
            bool connectionEstablished = (socket.Available == 0);
            if (connectionSent && connectionEstablished)
                return false;
            else
                return true;
        }
    }
}