using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class FavouriteSongService:IFavouriteSongService
    {
        private readonly IRepositoryWrapper _repository;

        public FavouriteSongService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FavouriteSong>> GetFavouriteSongsByUserIdAsync(string userId)
        {
            return await _repository.FavouriteSongRepository
                .FindByCondition(fs => fs.UserId == userId)
                .Include(fs => fs.Song)
                 .ThenInclude(s => s.Album)
                .ToListAsync();
        }

        public void AddFavouriteSong(string userId, int songId)
        {
            if (IsFavorited(userId, songId)) return;

            var favourite = new FavouriteSong
            {
                UserId = userId,
                SongId = songId
            };

            _repository.FavouriteSongRepository.Create(favourite);
            _repository.Save();
        }


        public bool IsFavorited(string userId, int songId)
        {
            return _repository.FavouriteSongRepository
                .FindByCondition(f => f.UserId == userId && f.SongId == songId)
                .Any();
        }

        public void RemoveFavouriteSong(string userId, int songId)
        {
            var favourite = _repository.FavouriteSongRepository
                .FindByCondition(f => f.UserId == userId && f.SongId == songId)
                .FirstOrDefault();

            if (favourite != null)
            {
                _repository.FavouriteSongRepository.Delete(favourite);
                _repository.Save();
            }
        }


    }
}
