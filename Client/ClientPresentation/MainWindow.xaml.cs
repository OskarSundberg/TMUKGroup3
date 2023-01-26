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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientBusiness.Model;
using ClientPresentation.ViewModels;

namespace ClientPresentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            ViewModel = new MainWindowViewModel();
            Client client = new Client();
            client.StartClient();
            InitializeComponent(); 
            
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            string msg = SendBox.Text;
            SendBox.Clear();
            Client c  = ViewModel.ClientList[0];
            c.SendMsg = msg;
        }
    }
}
