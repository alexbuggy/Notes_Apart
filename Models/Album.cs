using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace NotesApart.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string CoverImageUrl { get; set; }
        public string SpotifyAlbumId { get; set; }

        public Artist Artist { get; set; }
        public ICollection<Song>? Songs { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<FavouriteAlbum> FavouriteAlbums { get; set; }
    }
}
