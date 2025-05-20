using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class FavouriteAlbumService:IFavouriteAlbumService
    {
        private readonly IRepositoryWrapper _repository;

        public FavouriteAlbumService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FavouriteAlbum>> GetFavouriteAlbumsByUserIdAsync(string userId)
        {
            return await _repository.FavouriteAlbumRepository
                .FindByCondition(fa => fa.UserId == userId)
                .Include(fa => fa.Album)
                .ToListAsync();
        }

        public void AddFavouriteAlbum(string userId, int albumId)
        {
            if (IsFavorited(userId, albumId)) return;

            var favourite = new FavouriteAlbum
            {
                UserId = userId,
                AlbumId = albumId
            };

            _repository.FavouriteAlbumRepository.Create(favourite);
            _repository.Save();
        }


        public bool IsFavorited(string userId, int albumId)
        {
            return _repository.FavouriteAlbumRepository
                .FindByCondition(f => f.UserId == userId && f.AlbumId == albumId)
                .Any();
        }

        public void RemoveFavouriteAlbum(string userId, int albumId)
        {
            var favourite = _repository.FavouriteAlbumRepository
                .FindByCondition(f => f.UserId == userId && f.AlbumId == albumId)
                .FirstOrDefault();

            if (favourite != null)
            {
                _repository.FavouriteAlbumRepository.Delete(favourite);
                _repository.Save();
            }
        }
    }
}
