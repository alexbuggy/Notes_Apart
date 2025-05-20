using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album> GetAlbumByIdAsync(int albumId);
        Task<IEnumerable<Album>> GetTopRatedAlbumsAsync();
        Task<IEnumerable<Song>> GetSongsByAlbumIdAsync(int albumId);
        Task<double> GetAverageRatingForAlbumAsync(int albumId);
        Task<IEnumerable<Album>> SearchAlbumsAsync(string query);
        Task<Album> GetAlbumBySpotifyID(string spotifyId);

    }
}
