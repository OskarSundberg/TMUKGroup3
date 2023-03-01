using ClientBusiness.Model;
//using ClientPresentation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientPresentation;
using System.Windows.Input;
using System.Web;
using System.Windows.Media;


namespace ClientPresentation.ViewModels
{
    public class MainWindowViewModel
    {
        public IList<Client> UserClient { get; set; } = new ObservableCollection<Client>();
        public IList<Client> ClientList { get; } = new ObservableCollection<Client>();

        public Client UserClinet1 { get; set; }
        public Client Chosen_Client { get; set; }
        public string UserName { get; set; }
        public IList<Brush> ColorScheme { get; set; } = new ObservableCollection<Brush>();

        public MainWindowViewModel()
        {
            Client client = new Client();
            UserClient.Add(client);
            ColorScheme.Add(Brushes.Transparent);//Box
            ColorScheme.Add(Brushes.Black); //Button
            ColorScheme.Add(Brushes.Gray); //StackPanal
        }
    }
}
