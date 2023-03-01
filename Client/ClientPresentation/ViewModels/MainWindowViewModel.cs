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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClientPresentation.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public IList<Client> UserClient { get; set; } = new ObservableCollection<Client>();
        public IList<Client> ClientList { get; } = new ObservableCollection<Client>();

        public Client UserClinet1 { get; set; }
        public Client Chosen_Client { get; set; }
        public string UserName { get; set; }
        public ObservableCollection<Brush> ColorSchemeBox { get; set; } = new();
        public ObservableCollection<Brush> ColorSchemeButton { get; set; } = new ObservableCollection<Brush>();
        public ObservableCollection<Brush> ColorSchemeStackPanel { get; set; } = new ObservableCollection<Brush>();
        public ObservableCollection<Brush> ColorSchemeText { get; set; } = new ObservableCollection<Brush>();
        public Brush darkbrushe = (Brush)new BrushConverter().ConvertFrom("#333333");
        public Brush lightbrushe = (Brush)new BrushConverter().ConvertFrom("#ffffff");
        public MainWindowViewModel()
        {
            Client client = new Client();
            UserClient.Add(client);
            ColorSchemeBox.Add(lightbrushe);//Box
            ColorSchemeButton.Add(lightbrushe); //Button
            ColorSchemeStackPanel.Add(lightbrushe); //StackPanal
            ColorSchemeText.Add(Brushes.Black);//text
        }
        public void ColorScheme(string mode)
        {
            if (mode == "/darkmode")
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ColorSchemeBox[0] = darkbrushe;
                    ColorSchemeButton[0] = darkbrushe;
                    ColorSchemeStackPanel[0] = darkbrushe;
                    ColorSchemeText[0] = Brushes.White;
                });
            }
            else 
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ColorSchemeBox[0] = lightbrushe;
                    ColorSchemeButton[0] = lightbrushe;
                    ColorSchemeStackPanel[0] = lightbrushe;
                    ColorSchemeText[0] = Brushes.Black;
                });
            }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
