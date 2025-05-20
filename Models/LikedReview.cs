namespace NotesApart.Models
{
    public class LikedReview
    {

        public string UserId { get; set; }
        public User User { get; set; }

        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
