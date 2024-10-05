using LightPlayer.Models;
using LightPlayer.ViewModels;
using NAudio.Gui;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public class PlayMusicService : IPlayMusicService
    {
        public PlayMusicService()
        {
            Playlist = new();
            waveOut = new();
            Index = -1;

            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
        }

        private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (!flag)
            {
                if (PlayMode != PlayMode.SingleRepeat)
                {
                    if (Index == Playlist.Count - 1)
                    {
                        Index = -1;
                        if (PlayMode == PlayMode.OrderRepeat)
                            PlayNext();
                    }
                    else
                        PlayNext();
                }
                else
                    PlayAt(Index);
            }
            else
            {
                if(PlayMode == PlayMode.SingleRepeat)
                    PlayAt(Index);
                if (PlayMode == PlayMode.OrderPlay)
                    Index--;
                flag = false;
            }
        }

        private AudioFileReader audioFileReader;

        private WaveOut waveOut;
        public ObservableCollection<Song> Playlist {  get; set; }
        public TimeSpan? Position
        {
            get
            {
                TimeSpan? s;
                try
                {
                    s = audioFileReader?.CurrentTime;
                }
                catch
                {
                    return null;
                }
                return s;
            }
            set
            {
                if (value.HasValue)
                    audioFileReader.CurrentTime = (TimeSpan)value;
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                try
                {
                    return waveOut.PlaybackState;
                }
                catch
                {
                    return PlaybackState.Paused;
                }
            }
        }

        public TimeSpan? Length => audioFileReader?.TotalTime;

        private int index = -1;

        public int Index
        {
            get => index;
            set
            {
                if(index != value)
                {
                    index = value;
                    IndexChanged?.Invoke();
                }
            }
        }

        public float Volume
        {
            get => waveOut.Volume;
            set
            {
                if(waveOut != null)
                    waveOut.Volume = value;
            }
        }

        public PlayMode PlayMode { get; set; }

        public void Insert(Song song)
        {
            Playlist.Add(song);
        }

        public void InsertAndPlay(Song song)
        {
            Insert(song);
            PlayNext();
        }

        public void PlayAt(int index)
        {
            Index = index - 1;
            PlayNext();
        }

        public void Pause()
        {
            waveOut.Pause();
        }

        public void Play()
        {
            if (PlaybackState == PlaybackState.Paused)
                waveOut.Play();
            else
                PlayNext();
        }

        public void PlayNext()
        {
            Index++;
            waveOut.PlaybackStopped -= WaveOut_PlaybackStopped;
            waveOut.Stop();
            audioFileReader?.Dispose();
            audioFileReader = new AudioFileReader(Playlist[Index].Path);
            waveOut = null;
            waveOut = new();
            waveOut.Init(audioFileReader);
            waveOut.Play();
            waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            OnPlayNext?.Invoke(Playlist[Index]);
        }


        private bool flag = false;
        public void RemoveAt(int key)
        {
            if (Index < key)
                Playlist.RemoveAt(key);
            else if (Index > key)
            {
                Playlist.RemoveAt(key);
                Index--;
            }
            else
            {
                if (Index < Playlist.Count - 1)
                {
                    Playlist.RemoveAt(Index);
                    Index--;
                    PlayNext();
                }
                else
                {
                    Playlist.RemoveAt(Index);
                    Index = -1;
                    if(Playlist.Count != 0)
                    {
                        PlayNext();
                        Pause();
                    }
                    else
                    {
                        waveOut.Pause();
                    }
                }
                flag = true;
            }
        }

        public void Clear()
        {
            Index = -1;
            Playlist.Clear();
            PlayNext();
        }

        public event Action<Song>? OnPlayNext;

        public event Action? IndexChanged;
    }
}
