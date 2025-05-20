using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace NotesApart.Models
{
    public class Song
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public int? AlbumId { get; set; }
        public int TrackNumber { get; set; }
        public string SpotifySongId { get; set; }


        public Artist Artist { get; set; }
        public Album Album { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<FavouriteSong> FavouriteSongs { get; set; }
    }
}
