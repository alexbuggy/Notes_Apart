namespace NotesApart.Models
{
    public class FavouriteAlbum
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}
