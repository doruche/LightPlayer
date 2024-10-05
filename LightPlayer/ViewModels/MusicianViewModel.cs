using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LightPlayer.Models;
using LightPlayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.ViewModels
{
    public partial class MusicianViewModel : ViewModelBase, IRecipient<ValueChangedMessage<string>>
    {
        private readonly IMusicDBService musicDBService;

        public ObservableCollection<Musician> Musicians { get; set; }

        public MusicianViewModel(IMusicDBService musicDBService)
        {
            this.musicDBService = musicDBService;
            WeakReferenceMessenger.Default.Register(this);
            Musicians = new ObservableCollection<Musician>(musicDBService.GetAllMusicians());
        }

        [RelayCommand]
        private void Play(object e)
        {
            var song = e as Song;
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Song>(song!));
        }

        public void Receive(ValueChangedMessage<string> message)
            => Musicians = new ObservableCollection<Musician>(musicDBService.GetAllMusicians());
    }
}
