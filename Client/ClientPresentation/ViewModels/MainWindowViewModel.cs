using ClientBusiness.Model;
using ClientPresentation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientPresentation;
using System.Windows.Input;

namespace ClientPresentation.ViewModels
{
    public class MainWindowViewModel
    {
        public IList<Client> UserClient { get; set; } = new ObservableCollection<Client>();
        public IList<Client> ClientList { get; } = new ObservableCollection<Client>();

        public Client UserClinet1 { get; set; }
        public Client Chosen_Client { get; set; }
        public string UserName { get; set; }


        public MainWindowViewModel()
        {
            Client client = (new Client() { Name = "Dictator" });
            UserClient.Add(client);
            UserName = client.Name;
            ClientList.Add(new Client() { Name = "Ivo" });
            ClientList.Add(new Client() { Name = "Simon" });
            ClientList.Add(new Client() { Name = "Gustav" });
            ClientList.Add(new Client() { Name = "Samuel" });
        }


    }
}
