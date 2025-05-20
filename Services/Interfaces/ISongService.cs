using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface ISongService
    {
        Task<IEnumerable<Song>> GetAllSongsAsync();
        Task<Song> GetSongByIdAsync(int songId);
        Task<IEnumerable<Song>> GetTopRatedSongsAsync();
        Task<IEnumerable<Song>> SearchSongsAsync(string query);
        Task<Song> GetSongBySpotifyID(string spotifyId);
    }
}
