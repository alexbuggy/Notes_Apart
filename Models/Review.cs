namespace NotesApart.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public int Rating { get; set; }
        public int? SongId { get; set; }
        public int? AlbumId { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        public Song Song { get; set; }
        public Album Album { get; set; }
        public ICollection<LikedReview> LikedByUsers { get; set; }
    }
}
