using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace ClientPresentation.Views
{
    /// <summary>
    /// Interaction logic for EstablishConnection.xaml
    /// </summary>
    public partial class EstablishConnection : Window
    {

        private string ipAddress;

        private string name;

        private string portNumber;

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PortNumber
        {
            get { return portNumber; }
            set { portNumber = value; }
        }

        public EstablishConnection()
        {
            InitializeComponent();
            ErrorCode.Foreground = Brushes.White;

        }

        /// <summary>
        /// This method gets the IP from userinput in establishconnection window
        /// </summary>
        /// <param></param>
        /// <returns> Returns IPAdress </returns>
        private string SectionsToString()
        {
            string ipAddress;
            ipAddress = FirstIP.Text + "." + SecondIP.Text + "." + ThirdIP.Text + "." + FourthIP.Text;
            return ipAddress;
        }

        private void ConnectToServerClick(object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            ipAddress = SectionsToString();
            portNumber = portID.Text;
            name = userID.Text;
            EC.Hide();
        }

        private void MoveToNextBox(TextBox current)
        {
            current.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
        }

        private void MoveToPreviousBox(TextBox current)
        {
            current.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox current = (TextBox)sender;
            if (current.CaretIndex == 3)
                MoveToNextBox(current);
        }

        private void OnAction(object sender, KeyEventArgs e)
        {
            TextBox current = (TextBox)sender;
            if (e.Key == Key.Back && current.CaretIndex == 0 && current != FirstIP)
                MoveToPreviousBox(current);
            else if (e.Key == Key.Left && current != FirstIP && current.CaretIndex == 0 && !(current == FourthIP && current.CaretIndex != 0))
                MoveToPreviousBox(current);
            else if (e.Key == Key.Right && current != FourthIP && current.CaretIndex == 3 && !(current == FirstIP && current.CaretIndex != 3))
                MoveToNextBox(current);
        }

        private void OnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConnectToServer();
        }
    }
}
