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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LightPlayer.ViewModels
{
    public partial class MusicViewModel : ViewModelBase
    {
        private readonly IMusicDBService musicDBService;

        private readonly IGetFileService getFileService;
        private readonly IPlayMusicService playMusicService;

        [ObservableProperty]
        private ObservableCollection<Song> songs;

        public bool IsEmpty { get; set; }
        public bool IsNotEmpty { get; set; }

        public MusicViewModel(IMusicDBService musicDBService, IGetFileService getFileService, IPlayMusicService playMusicService)
        {
            this.musicDBService = musicDBService;
            this.getFileService = getFileService;
            this.playMusicService = playMusicService;

            Songs = new ObservableCollection<Song>(musicDBService.GetAllSongs());

            Songs.CollectionChanged += (_, _) =>
            {
                IsEmpty = Songs.Count == 0;
                IsNotEmpty = !IsEmpty;
                OnPropertyChanged(nameof(IsEmpty));
                OnPropertyChanged(nameof(IsNotEmpty));
            };
            IsEmpty = Songs.Count == 0;
            IsNotEmpty = !IsEmpty;
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));


            this.getFileService = getFileService;
        }


        [RelayCommand]
        private void Load()
        {
            if (getFileService.OpenFileDialog(out string[] paths))
                foreach (string path in paths)
                {
                    if (musicDBService.AddSong(path))
                    {
                        Songs.Add(musicDBService.GetSong(path));
                        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>("UpdateSongs"));
                    }
                }
        }

        [RelayCommand]
        private void Play(object e)
        {
            var song = e as Song;
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Song>(song!));
        }
    }
}
