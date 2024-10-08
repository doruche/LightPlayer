using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LightPlayer.Models;
using LightPlayer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.ViewModels
{
    public partial class SearchPageViewModel : ViewModelBase, IRecipient<ValueChangedMessage<string>>
    {
        private readonly IMusicDBService musicDBService;
        [ObservableProperty]
        private string filterText;

        [ObservableProperty]
        private ObservableCollection<Song> results;

        public bool IsEmpty { get; set; }
        public bool IsNotEmpty { get; set; }

        public SearchPageViewModel(IMusicDBService musicDBService)
        {
            this.musicDBService = musicDBService;
            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(ValueChangedMessage<string> message)
        {
            if (message.Value.Equals("ToOpen"))
                return;
            FilterText = message.Value;
            Results = new(musicDBService.GetAllSongs().Where(s => Song.IsRelated(s, FilterText!)));
            IsEmpty = Results.Count == 0;
            IsNotEmpty = !IsEmpty;
            
        }

        [RelayCommand]
        private void Play(object e)
        {
            var song = e as Song;
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Song>(song!));
        }
    }
}
