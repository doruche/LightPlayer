using TagLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LightPlayer.Models
{
    public class MusicDB
    {
        public static Lazy<MusicDB> Instance { get; set; } = new(() => new());

        private Dictionary<string, Musician> musicians = new();

        private Dictionary<string, Album> albums = new() { };

        private Dictionary<string, Song> songs = new();

        private void UpdateMusician(string name, string path)
        {
            if (!musicians.ContainsKey(name))
                musicians[name] = new(name);
            var musician = musicians[name];
            musician.Songs!.Add(songs[path]);
        }

        private void UpdateAlbum(string name, string path)
        {
            if (!albums.ContainsKey(name))
                albums[name] = new(name);
            var album = albums[name];
            if(!string.IsNullOrEmpty(path))
                album.Icon = path;
            album.Songs!.Add(songs[path]);
        }

        public bool AddSong(string path)
        {
            if (songs.ContainsKey(path))
                return false;
            TagLib.File song = TagLib.File.Create(path);
            songs.Add(path,  new(
                path,
                song.Tag.Title,
                song.Properties.Duration,
                song.Tag.FirstPerformer,
                song.Tag.Album));
            UpdateMusician(song.Tag.FirstPerformer, path);
            if (song.Tag.Pictures.Length > 0)
                UpdateAlbum(song.Tag.Album, path);
            else
                UpdateAlbum(song.Tag.Album, string.Empty);
            return true;
        }

        public bool RemoveSong(string path) => songs.Remove(path);

        public IEnumerable<Song> GetAllSongs() => songs.Select(x=>x.Value);

        public Song GetSong(string path) => songs[path];

        public IEnumerable<Musician> GetAllMusicians() => musicians.Select(x=>x.Value);

        public IEnumerable<Album> GetAllAlbums() => albums.Select(x=>x.Value);

    }
}
