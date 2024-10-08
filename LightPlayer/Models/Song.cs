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
        public static bool IsRelated(Song song, string filter)
        {
            if (string.IsNullOrEmpty(filter))
                return false;

            var a = song.Name.Contains(filter);
            var b = song.Musician?.Contains(filter);
            var c = song.Album?.Contains(filter);

            if (b.HasValue && c.HasValue)
                return a || b.Value || c.Value;
            else if (b.HasValue)
                return a || b.Value;
            else if (c.HasValue)
                return a || c.Value;
            else
                return a;
        }

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
