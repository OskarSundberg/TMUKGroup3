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

        //callback 
        private Action<string> _massageCallBack;

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
        public void StartClient(ConectionInfo cInfo, Action<string> massageCallBack)
        {


            try
            {
                //IPAddress ipAddress = IPAddress.Parse(Ip);
                IPEndPoint server = new IPEndPoint(cInfo.IP, 13375);

                Sender = new Socket(cInfo.IP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                this._massageCallBack = massageCallBack;

                try
                {
                    Sender.Connect(server);
                    byte[] cUseName = Encoding.UTF8.GetBytes(cInfo.UserName);
                    Sender.Send(cUseName);
                    Console.WriteLine("Socket connected to {0}", Sender.RemoteEndPoint.ToString());

                    Thread recieveMessageThread = new Thread(RecieveMessageFromServer);
                    recieveMessageThread.IsBackground = true;
                    recieveMessageThread.Start();
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

        public void GetMessageFromClient(string msgstr)
        {
            byte[] msg = Encoding.UTF8.GetBytes(msgstr + char.ToString('\u009F'));
            int bytesSent = Sender.Send(msg);
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
                    this._massageCallBack(Encoding.UTF8.GetString(bytes, 0, bytesRec));
                }
            }
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}