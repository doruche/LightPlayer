using LightPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public class MusicDBService : IMusicDBService
    {
        public bool AddSong(string path) => MusicDB.Instance.Value.AddSong(path);

        public bool RemoveSong(string path) => MusicDB.Instance.Value.RemoveSong(path);

        public void Initiate() => MusicDB.Instance.Value.Initiate();

        public IEnumerable<Song> GetAllSongs() => MusicDB.Instance.Value.GetAllSongs();

        public Song GetSong(string path) => MusicDB.Instance.Value.GetSong(path);

        public IEnumerable<Musician> GetAllMusicians() => MusicDB.Instance.Value.GetAllMusicians();

        public IEnumerable<Album> GetAllAlbums() => MusicDB.Instance.Value.GetAllAlbums();
    }
}
