using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Models
{
    public partial class Song : ObservableObject
    {
        public string Path {  get; set; }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private TimeSpan time;

        [ObservableProperty]
        private string? musician;

        [ObservableProperty]
        private string? album;

        public Song(string path, string name, TimeSpan time, string? musician = null, string? album = null)
        {
            Path= path;
            Name= name;
            Musician= musician;
            Album= album;
            Time = time;
        }
    }
}
