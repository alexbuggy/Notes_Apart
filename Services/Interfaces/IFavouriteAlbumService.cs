using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IFavouriteAlbumService
    {
        Task<IEnumerable<FavouriteAlbum>> GetFavouriteAlbumsByUserIdAsync(string userId);
        void AddFavouriteAlbum(string userId,int albumId);
        void RemoveFavouriteAlbum(string userId, int albumId);
        bool IsFavorited(string userId, int albumId);
    }
}
