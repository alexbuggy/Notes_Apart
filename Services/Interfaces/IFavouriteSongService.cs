using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IFavouriteSongService
    {
        Task<IEnumerable<FavouriteSong>> GetFavouriteSongsByUserIdAsync(string userId);
        void AddFavouriteSong(string userId, int song);
        void RemoveFavouriteSong(string userId, int songId);
        bool IsFavorited(string userId, int songId);
    }
}
