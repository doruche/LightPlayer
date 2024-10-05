using LightPlayer.Models;
using LightPlayer.ViewModels;
using Microsoft.Xaml.Behaviors.Core;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public interface IPlayMusicService
    {
        public ObservableCollection<Song> Playlist { get; set; }

        public TimeSpan? Position {  get; set; }

        public TimeSpan? Length { get; }

        public PlaybackState PlaybackState { get; }

        public PlayMode PlayMode { get; set; }

        public float Volume { get; set; }

        public int Index { get; set; }

        public void Play();

        public void PlayNext();

        public void PlayAt(int index);

        public void Pause();

        public void Insert(Song song);

        public void InsertAndPlay(Song song);

        public void RemoveAt(int key);

        public void Clear();

        public event Action<Song>? OnPlayNext;

        public event Action? IndexChanged;

    }
}
