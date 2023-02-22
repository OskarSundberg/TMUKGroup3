using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientBusiness.Model;
using ClientPresentation.ViewModels;
using ClientPresentation.Views;

namespace ClientPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientPresentation.MainWindow GetMainWindow { get; set; }
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
            EstablishConnection ec = new EstablishConnection();
            ec.ShowDialog();
            ConnectionInfo cInfo = new ConnectionInfo(IPAddress.Parse(ec.IpAddress), Int32.Parse(ec.PortNumber), ec.Name);
            ViewModel = new MainWindowViewModel();
            ViewModel.UserClient[0].Name = ec.Name;
            Client client = new Client();
            client.StartClient(cInfo, ServerMessage);
            Time = DateTime.Now;
            InitializeComponent();
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
            //stops user from sendig a empty message and the samme twice in a row
            if (msg != "" && msg != OldMessage)
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
    }
}
