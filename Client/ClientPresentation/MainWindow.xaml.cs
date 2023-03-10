using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientBusiness.Model;
using ClientPresentation.ViewModels;
using ClientPresentation.Views;
using System.Windows.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow mainWindow { get; set; }
        public MainWindowViewModel ViewModel { get; set; }
        /// <summary>
        /// Used to determine the time from the last meassage sent
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// Used stop the user to send the same meassage twice in a row
        /// </summary>
        public string OldMessage { get; set; }

        public MainWindow()
        {
            Client client = new Client();
            EstablishConnection ec = new EstablishConnection();
            ViewModel = new MainWindowViewModel();
            ec.ShowDialog();
            ConnectionInfo cInfo = new ConnectionInfo(ec.IpAddress, Int32.Parse(ec.PortNumber), ec.Name);
            ViewModel.UserClient[0].Name = ec.Name;
            client.StartClient(cInfo, ServerMessage, UppdateUsersOnlinePanel);
            if (!Client.Sender.Connected)
                ec.ErrorCode.Text = ec.errormsg[1];
            while (!Client.Sender.Connected)
            {
                ec.ShowDialog();
                cInfo.IP = ec.IpAddress;
                cInfo.Port = Int32.Parse(ec.PortNumber);
                cInfo.UserName = ec.Name;
                ViewModel.UserClient[0].Name = ec.Name;
                client.StartClient(cInfo, ServerMessage, UppdateUsersOnlinePanel);
            }
            ec.Hide();
            Time = DateTime.Now;
            Thread checkConnection = new Thread(() => CheckConnection(Client.Sender));
            checkConnection.Start();
            checkConnection.IsBackground = true;
            InitializeComponent();
            mainWindow = Window.GetWindow(App.Current.MainWindow) as MainWindow;
        }
        public MainWindow(string test)
        {
            OldMessage = "hello";
            ViewModel = new MainWindowViewModel();
            ViewModel.UserClient[0].Name = "test";
            InitializeComponent();
            SendBox.Text = test;
        }

        private void ServerMessage(string message)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MessagesBox.AppendText($"{message}\n");
                MessageBoxScrollBar.ScrollToEnd();
            });
        }

        /// <summary>
        /// Sends whats in the send box when Save button clicked.
        /// </summary>
        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            if (true == SpamFilter())
            {
                string msg = SendBox.Text;
                SendBox.Clear();
                Send_message(msg);
            }
        }

        /// <summary>
        /// Sends what is in the send boc when enter is pressed.
        /// </summary>
        private void SendBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SpamFilter())
                {
                    string msg = SendBox.Text;
                    SendBox.Clear();
                    Send_message(msg);
                }
                else
                {
                    ServerMessage("BotenAnna: slow down partner! You can't send more then one message a second!");
                }
            }
        }

        /// <summary>
        /// Used to send the input to server
        /// </summary>
        public bool Send_message(string msg)
        {
            if (msg == "/help")
            {
                ServerMessage("BotenAnna: " +
                              "\n===========================================================" +
                              "\n/online \t\t\t  -> Gives a list of users online." +
                              "\n===========================================================" +
                              "\n/whisper [user_tag]\t -> Private chat with the user you choose.s" +
                              "\n===========================================================" +
                              "\n/unicorn\t\t\t -> Unicorn in the chat for all to see." +
                              "\n===========================================================" +
                              "\n/emoji  \t\t\t  -> List of possible emojis in the chat." +
                              "\n===========================================================" +
                              "\n/darkmode \t\t -> Set darkmode." +
                              "\n===========================================================" +
                              "\n/lightmode\t\t -> Set lightmode.");

            }
            else if (msg == "/darkmode")
            {
                mainWindow.ClinetViewModel.ColorScheme(msg);
            }
            else if (msg == "/lightmode")
            {
                mainWindow.ClinetViewModel.ColorScheme(msg);
            }
            //stops user from sendig a empty message and the samme twice in a row
            else if (msg != "" && msg != OldMessage)
            {
                Client c = ViewModel.UserClient[0];
                c.SendMsg = msg;
                OldMessage = msg;
                Time = DateTime.Now;
                return true;
            }
            else
                ServerMessage("BotenAnna: You are not allowed to spam in the chat! (ง•o•)ง");
            return false;
        }


        /// <summary>
        /// Stops the user from sending more the one message a second 
        /// </summary>
        /// <returns>  true :if more the a second has past</returns>
        /// <returns> false :if less the a second has past</returns>
        public bool SpamFilter()
        {
            if (Time.AddMilliseconds(1000) > DateTime.Now)
            {
                return false;
            }
            else
                Time = DateTime.Now;
            return true;
        }

        public void CheckConnection(Socket s)
        {
            while (s.Connected)
                Thread.Sleep(3000);
            App.Current.Dispatcher.Invoke(() =>
            {
                MessagesBox.AppendText("Server has shut down. Click connect to retry connection");
            });
        }

        public void UppdateUsersOnlinePanel(string[] users)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                UsersOnlinePanel.Children.Clear();
                foreach (string user in users)
                {
                    Button button = new Button();
                    button.Content = user;
                    button.Foreground = Brushes.White;
                    Brush brushe = (Brush)new BrushConverter().ConvertFrom("#333333");
                    button.Background = brushe;
                    button.Name = user;
                    button.Click += (sender, e) =>
                    {
                        Button b = (Button)sender;
                        SendBox.AppendText("/whisper/" + b.Name.ToString() + "/");
                    };
                    UsersOnlinePanel.Children.Add(button);
                }
            });
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
