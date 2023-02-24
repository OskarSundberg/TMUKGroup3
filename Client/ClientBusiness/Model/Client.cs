using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;

namespace ClientBusiness.Model
{


    public class Client : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string name;

        MessageHandler msgHandler = new();
        public static Socket Sender { get; set; }
        public static Socket DataSender { get; set; }

        //callback
        private Action<string> _messageCallBack;
        private Action<string[]> _uppdateUserOnlnePanel;

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

                GetMessageFromClient(value);
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
        public void StartClient(ConnectionInfo cInfo, Action<string> massageCallBack, Action<string[]> UsersOnlnePanel)
        {
            try
            {
                IPEndPoint server = new IPEndPoint(cInfo.IP, cInfo.Port);

                Sender = new Socket(cInfo.IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._messageCallBack = massageCallBack;
                this._uppdateUserOnlnePanel = UsersOnlnePanel;

                try
                {
                    Sender.Connect(server);
                    if (!SocketConnected(Sender))
                        throw new Exception("Not Connected");

                    byte[] cUseName = Encoding.UTF8.GetBytes(cInfo.UserName);
                    Sender.Send(cUseName);
                    Console.WriteLine($"Socket connected to {Sender.RemoteEndPoint.ToString()}");

                    byte[] bytes = new byte[64000];
                    IPAddress ip = IPAddress.Parse("127.0.0.1");
                    string dataPort = "31337";
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
                    Console.WriteLine($"Exception : {e.ToString()}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception : {e.ToString()}");
            }
        }

        public void GetMessageFromClient(string msgstr)
        {
            try
            {
                MsgPacket.Message msg = new(msgstr, name);
                byte[] bytes = msgHandler.SerializeMsg(msg);
                int bytesSent = Sender.Send(bytes);
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
                    MsgPacket.Message msg = msgHandler.DeserializeMsg(bytes, bytesRec);
                    this._messageCallBack(msg.UserFrom + ": " + msg.Msg);
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
                    string msg = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    string[] usersOnline = msg.Split(',');
                    this._uppdateUserOnlnePanel(usersOnline);
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