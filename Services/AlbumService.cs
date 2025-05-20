using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IRepositoryWrapper _repository;

        public AlbumService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _repository.AlbumRepository.FindAll().ToListAsync();
        }

        public async Task<Album> GetAlbumByIdAsync(int albumId)
        {
            return await _repository.AlbumRepository.FindByCondition(a => a.AlbumId == albumId)
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .Include(a => a.Reviews)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Album>> GetTopRatedAlbumsAsync()
        {
            return await _repository.AlbumRepository.FindAll()
                .Include(a => a.Artist) 
                .OrderByDescending(a => a.Reviews.Average(r => (double?)r.Rating) ?? 0)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> GetSongsByAlbumIdAsync(int albumId)
        {
            var songs = await _repository.SongRepository.FindByCondition(s => s.AlbumId == albumId).ToListAsync();
            return songs;
        }

        public async Task<double> GetAverageRatingForAlbumAsync(int albumId)
        {
            var reviews = await _repository.ReviewRepository.FindByCondition(r => r.AlbumId == albumId).ToListAsync();
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            return averageRating;
        }

        public async Task<IEnumerable<Album>> SearchAlbumsAsync(string query)
        {
            return await _repository.AlbumRepository
                .FindByCondition(a => a.Title.Contains(query))
                .Include(a => a.Artist)
                .ToListAsync();
        }

        public async Task<Album> GetAlbumBySpotifyID(string spotifyId)
        {
           return await _repository.AlbumRepository
                .FindByCondition(a => a.SpotifyAlbumId == spotifyId)
                   .FirstOrDefaultAsync();
        }



    }
}
