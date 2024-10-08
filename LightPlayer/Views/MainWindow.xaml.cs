using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LightPlayer.Behaviours;
using LightPlayer.ViewModels;
using System.Text;
using System.Windows;


namespace LightPlayer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IRecipient<ValueChangedMessage<string>>
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            WeakReferenceMessenger.Default.Register(this);
        }

        private void PlaylistButtonClick(object sender, RoutedEventArgs e)
        {
            if(playlistBorder.Visibility == Visibility.Visible)
                playlistBorder.Visibility = Visibility.Collapsed;
            else
                playlistBorder.Visibility = Visibility.Visible;
        }

        public void Receive(ValueChangedMessage<string> message)
        {
            if (message.Value == "ToOpen")
                this.splitView.IsPaneOpen = true;
        }

    }
}