using LightPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public interface IMusicDBService
    {
        public bool AddSong(string path);

        public bool RemoveSong(string path);

        public void Initiate();

        public IEnumerable<Song> GetAllSongs();

        public Song GetSong(string path);

        public IEnumerable<Musician> GetAllMusicians();

        public IEnumerable<Album> GetAllAlbums();
    }
}
