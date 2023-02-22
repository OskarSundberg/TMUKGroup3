using System;
using System.Collections.Generic;
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
            ipAddress = SectionsToString();
            portNumber = portID.Text;
            name = userID.Text;
            EC.Close();
        }

        private void MoveToNextBox(TextBox current)
        {
            if (current.GetLineLength(0) == 3)
            {

            }

        }


    }
}
