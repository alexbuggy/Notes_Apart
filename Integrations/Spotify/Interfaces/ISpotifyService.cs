using NotesApart.Models;

namespace NotesApart.Integrations.Spotify.Interfaces
{
    public interface ISpotifyService
    {

            Task SeedInitialDataAsync();
            Task<List<Artist>> FetchArtistsByNameAsync(string name);
            Task<Album> FetchAlbumByNameAsync(string name);
            Task<Song> FetchSongByNameAsync(string name);
        
    }
}
