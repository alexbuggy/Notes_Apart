using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetAllArtistsAsync();
        Task<Artist> GetArtistByIdAsync(int artistId);
        Task<Artist> GetArtistBySpotifyID(string spotifyId);
        Task<List<Album>> GetAllAlbumsByArtistAsync(int artistId);
        Task<double> GetAverageAlbumRatingByArtistIdAsync(int artistId);
        Task<double> GetAverageRatingForArtistAsync(int artistId);
        Task<IEnumerable<Artist>> SearchArtistsAsync(string query);

    }
}