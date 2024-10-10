using TagLib;
using System.Text.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.Immutable;
using NAudio.CoreAudioApi.Interfaces;
using System.Text.Json.Nodes;
using System.Windows.Shapes;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization.Metadata;

namespace LightPlayer.Models
{
    public class MusicDB
    {
        public static Lazy<MusicDB> Instance { get; set; } = new(() => new());

        private Dictionary<string, Musician> musicians = new();

        private Dictionary<string, Album> albums = new();

        private Dictionary<string, Song> songs = new();

        public void Initiate()
        {
            if (!System.IO.File.Exists("songs.json"))
                System.IO.File.WriteAllText("songs.json", "[]");
            var data = JsonSerializer.Deserialize<List<Song>>(System.IO.File.ReadAllText("songs.json"));
            if (data!.Count == 0)
                return;
            foreach(var item in data)
            {
                if (System.IO.File.Exists(item.Path))
                {
                    TagLib.File song = TagLib.File.Create(item.Path);
                    songs.Add(item.Path, new(
                        item.Path,
                        song.Tag.Title,
                        song.Properties.Duration,
                        string.Join(", ", song.Tag.Performers),
                        song.Tag.Album));
                    foreach (var musician in song.Tag.Performers)
                        UpdateMusician(musician, item.Path);

                    if (song.Tag.Pictures.Length > 0)
                        UpdateAlbum(song.Tag.Album, item.Path);
                    else
                        UpdateAlbum(song.Tag.Album, string.Empty);
                }
            }

            data = songs.Values.ToList();
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions() 
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            });
            System.IO.File.Delete("songs.json");
            System.IO.File.WriteAllText("songs.json", json);
        }

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
                string.Join(", ", song.Tag.Performers),
                song.Tag.Album));
            foreach(var musician in song.Tag.Performers)
                UpdateMusician(musician, path);
            if (song.Tag.Pictures.Length > 0)
                UpdateAlbum(song.Tag.Album, path);
            else
                UpdateAlbum(song.Tag.Album, string.Empty);

            var data = JsonNode.Parse(System.IO.File.ReadAllText("songs.json"));
            data!.AsArray().Add<Song>(new(
                path,
                song.Tag.Title,
                song.Properties.Duration,
                string.Join(", ", song.Tag.Performers),
                song.Tag.Album));
            System.IO.File.Delete("songs.json");
            System.IO.File.WriteAllText("songs.json", data.ToJsonString(new JsonSerializerOptions() 
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                TypeInfoResolver = JsonSerializerOptions.Default.TypeInfoResolver
            }));

            return true;
        }

        public bool RemoveSong(string path) => songs.Remove(path);

        public IEnumerable<Song> GetAllSongs() => songs.Select(x=>x.Value);

        public Song GetSong(string path) => songs[path];

        public IEnumerable<Musician> GetAllMusicians() => musicians.Select(x=>x.Value);

        public IEnumerable<Album> GetAllAlbums() => albums.Select(x=>x.Value);

    }
}
