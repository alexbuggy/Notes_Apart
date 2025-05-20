using NotesApart.Models;

namespace NotesApart.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Album> TopRatedAlbums { get; set; }
        public IEnumerable<Song> TopRatedSongs { get; set; }
        public IEnumerable<Review> TopReviews { get; set; }
        public List<int> LikedReviewIds { get; set; } = new List<int>();
    }
}
