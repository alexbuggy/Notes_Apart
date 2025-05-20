namespace NotesApart.Models
{
    public class Artist
    {
            public int ArtistId { get; set; }
            public string Name { get; set; }
            public string SpotifyArtistId { get; set; }
            public string ImageUrl { get; set; }


            public ICollection<Album>? Albums { get; set; }
            public ICollection<Song> Songs { get; set; }
    }
}
