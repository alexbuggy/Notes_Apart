using NotesApart.Models;

namespace NotesApart.ViewModels
{
    public class AlbumDetailsViewModel
    {
        public Album Album { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public bool IsFavorited { get; set; }
        public List<int> LikedReviewIds { get; set; } = new List<int>();
    }
}
