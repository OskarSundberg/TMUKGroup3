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

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public EstablishConnection()
        {
            InitializeComponent();
        }

        private string SectionsToString()
        {
            string ipAddress;

            ipAddress = FirstIP.Text + "." + SecondIP.Text + "." + ThirdIP.Text + "." + FourthIP.Text;

            return ipAddress;
        }

        private void ConnectToServerClick(object sender, RoutedEventArgs e)
        {
            ipAddress = SectionsToString();
            EC.Close();
        }

        private void MoveToNextBox(TextBox current)
        {
            if(current.GetLineLength(0) == 3)
            {

            }

        }


    }
}
