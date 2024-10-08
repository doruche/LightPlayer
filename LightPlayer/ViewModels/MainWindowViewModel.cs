using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LightPlayer.Models;
using LightPlayer.Services;
using LightPlayer.Views;
using NAudio.Wave;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Pipes;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace LightPlayer.ViewModels
{
    public partial class MainWindowViewModel : ObservableRecipient,
                                               IRecipient<ValueChangedMessage<Song>>,
                                               IRecipient<ValueChangedMessage<ObservableCollection<Song>>>
    {
        private readonly NavigationService navigationService;

        private readonly IPlayMusicService playMusicService;

        private readonly IMusicDBService musicDBService;

        private readonly IGetFileService getFileService;

        [ObservableProperty]
        private ObservableCollection<Song> songsSource;

        [ObservableProperty]
        private List<Song> filterSongs;

        [ObservableProperty]
        private ViewModelBase? currentViewModel;

        [ObservableProperty]
        private string? filterText;

        private Song? currentSong;

        public Song? CurrentSong
        {
            set
            {
                if(currentSong != value)
                {
                    currentSong = value;
                    OnPropertyChanged();
                    UpdateInfo();
                }
            }
            get => currentSong;
        }

        [ObservableProperty]
        private PlayMode playMode;

        [ObservableProperty]
        private TimeSpan? position;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? musician;

        [ObservableProperty]
        private TimeSpan? length;

        public PlaybackState? PlaybackState => playMusicService.PlaybackState;

        [ObservableProperty]
        private double? progress;

        [ObservableProperty]
        private float volume;
        public int Index => playMusicService.Index;

        [ObservableProperty]
        private BitmapSource? image;

        [ObservableProperty]
        private string playButtonImage = "pack://application:,,,/Properties/Play.png";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(PlayCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveNextCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePreviousCommand))]
        private bool isPlayable;

        [ObservableProperty]
        private ObservableCollection<Song>? playlist;

        private readonly System.Timers.Timer timer = new(500);
        
        public List<TabView> Tabs { get; set; } = new List<TabView>
        {
            new("Music"),
            new("Musician"),
            new("Album"),
            new("Playlist"),
        };

        public MainWindowViewModel(NavigationService navigationService,
            IPlayMusicService playMusicService, 
            IMusicDBService musicDB,
            IGetFileService getFileService)
        {
            this.navigationService = navigationService;
            this.playMusicService = playMusicService;
            this.musicDBService = musicDB;
            this.getFileService = getFileService;

            this.IsActive = true;
            this.PlayMode = ViewModels.PlayMode.OrderPlay;

            navigationService.CurrentViewModelChanged += () => CurrentViewModel = navigationService.CurrentViewModel;
            navigationService.NavigateTo<MusicViewModel>();

            timer.Elapsed += (_, _) =>
            {
                Position = playMusicService.Position;
                Progress = 10 * Position / playMusicService.Length;
                playMusicService.Volume = Volume;
            };
            timer.Start();

            this.Playlist = playMusicService.Playlist;
            playMusicService.PlayMode = ViewModels.PlayMode.OrderPlay;
            playMusicService.OnPlayNext += x => CurrentSong = x;
            playMusicService.IndexChanged += () => OnPropertyChanged(nameof(Index));
            Playlist.CollectionChanged += (_, _) =>
            {
                if (Playlist.Count > 0)
                {
                    if(timer.Enabled == false)
                        timer.Start();
                    IsPlayable = true;
                }
                else if (Playlist.Count == 0)
                {
                    IsPlayable = false;
                    timer.Stop();
                }
            };
        }



        private void UpdateInfo()
        {
            if (CurrentSong != null)
            {
                TagLib.File file = TagLib.File.Create(CurrentSong.Path);
                Title = file.Tag.Title;
                Musician = file.Tag.FirstPerformer;
                Length = file.Properties.Duration;
                if (file.Tag.Pictures.Length > 0)
                    Image = GetBitmapSource(file.Tag.Pictures[0].Data.Data);
            }
            else
            {
                Title = null;
                Musician = null;
                Length = null;
                Image = null;
            }
    
            BitmapSource GetBitmapSource(byte[] b)
            {
                using (MemoryStream stream = new MemoryStream(b))
                {
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();

                    // This is important: the image should be loaded on setting the StreamSource property, afterwards the stream will be closed
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.DecodePixelWidth = 160;

                    // Set this to 160 to get exactly 160x160 image
                    // Comment it out to retain original aspect ratio having the image width 160 and auto calculated image height
                    bi.DecodePixelHeight = 160;

                    bi.StreamSource = stream;
                    bi.EndInit();
                    return bi;
                }
            }

        }

        [RelayCommand(CanExecute =nameof(IsPlayable))]
        private void Play()
        {
            if (playMusicService.PlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                playMusicService.Pause();
                timer.Stop();
                PlayButtonImage = "pack://application:,,,/Properties/Play.png";
            }
            else
            {
                playMusicService.Play();
                timer.Start();
                PlayButtonImage = "pack://application:,,,/Properties/Pause.png";
            }
        }

        [RelayCommand(CanExecute = nameof(IsPlayable))]
        private void MoveNext()
        {
            if (playMusicService.Index == playMusicService.Playlist.Count - 1)
                playMusicService.Index = -1;
            playMusicService.PlayNext();
        }

        [RelayCommand(CanExecute = nameof(IsPlayable))]
        private void MovePrevious()
        {
            if (playMusicService.Index == -1 || playMusicService.Index == 0)
                playMusicService.Index = playMusicService.Playlist.Count - 2;
            else
                playMusicService.Index -= 2;
            playMusicService.PlayNext();
        }

        [RelayCommand]
        private void PlayFromList(int index)
        {
            playMusicService.PlayAt(index);
            CurrentSong = playMusicService.Playlist[index];
            PlayButtonImage = "pack://application:,,,/Properties/Pause.png";
        }

        [RelayCommand]
        private void RemoveFromList(int key)
        {
            playMusicService.RemoveAt(key);
            if(Index != -1)
                CurrentSong = playMusicService.Playlist[Index];
            else
                CurrentSong = null;
        }

        [RelayCommand]
        private void ChangePlayMode()
        {
            if (PlayMode == ViewModels.PlayMode.SingleRepeat)
                PlayMode = ViewModels.PlayMode.OrderPlay;
            else
                PlayMode++;
            playMusicService.PlayMode = PlayMode;
        }

        public void Receive(ValueChangedMessage<Song> message)
        {
            CurrentSong = message.Value;
            PlayButtonImage = "pack://application:,,,/Properties/Pause.png";

            playMusicService.InsertAndPlay(message.Value);
        }

        public void Receive(ValueChangedMessage<ObservableCollection<Song>> message) => SongsSource = message.Value;

        [RelayCommand]
        private void SliderMouseLeftButtonDown()
        {
            timer.Stop();
        }

        [RelayCommand]
        private void SliderMouseLeftButtonUp()
        {
            Position = Progress / 10 * Length;
            playMusicService.Position = Position;
            timer.Start();
        }

        [RelayCommand]
        private void Search(bool isPaneOpen)
        {
            if (!isPaneOpen)
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>("ToOpen"));
            else
            {
                navigationService.GoToSearchPage(FilterText);
            }
        }
    }

    public enum PlayMode
    {
        OrderPlay,
        OrderRepeat,
        SingleRepeat
    }
}
