namespace NotesApart.Models
{
    public class FavouriteSong
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int SongId { get; set; }


        public Song Song { get; set; }
    }
}
