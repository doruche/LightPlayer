using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Models
{
    public partial class Musician : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<Song>? songs;

        [ObservableProperty]
        private ObservableCollection<Album>? albums;

        public static readonly Musician Unknown = new("Unknown");

        public Musician(string name)
        {
            Name = name;
            songs = new();
            albums = new();
        }
    }
}
