using NotesApart.Models;

namespace NotesApart.ViewModels
{
    public class ProfileViewModel
    {
        public List<FavouriteAlbum> FavouriteAlbums { get; set; }
        public List<FavouriteSong> FavouriteSongs { get; set; }
        public List<Review> Reviews { get; set; }
        public List<int> LikedReviewIds { get; set; } = new List<int>();
    }
}
