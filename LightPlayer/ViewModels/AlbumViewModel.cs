using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using LightPlayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightPlayer.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LightPlayer.ViewModels
{
    public partial class AlbumViewModel : ViewModelBase, IRecipient<ValueChangedMessage<string>>
    {
        private readonly IMusicDBService musicDBService;

        [ObservableProperty]
        private ObservableCollection<Album> albums;

        public AlbumViewModel(IMusicDBService musicDBService)
        {
            this.musicDBService = musicDBService;
            Albums = new(musicDBService.GetAllAlbums());
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(ValueChangedMessage<string> message)
            => Albums = new(musicDBService.GetAllAlbums());

        [RelayCommand]
        private void Play(object e)
        {
            var song = e as Song;
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Song>(song!));
        }
    }
}
