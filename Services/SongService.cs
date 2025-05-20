using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class SongService : ISongService
    {
        private readonly IRepositoryWrapper _repository;

        public SongService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Song>> GetAllSongsAsync()
        {
            return await _repository.SongRepository.FindAll()
                .Include(s => s.Artist) 
                .Include(s => s.Album)  
                .ToListAsync();
        }

        public async Task<Song> GetSongByIdAsync(int songId)
        {
            return await _repository.SongRepository.FindByCondition(s => s.SongId == songId)
                .Include(s => s.Artist) 
                .Include(s => s.Album)  
                .Include(s => s.Reviews)
                .FirstOrDefaultAsync();
        }

        public async Task<Song> GetSongBySpotifyID(string spotifyId)
        {
            return await _repository.SongRepository
            .FindByCondition(a => a.SpotifySongId == spotifyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Song>> GetTopRatedSongsAsync()
        {
            return await _repository.SongRepository.FindAll()
                .Include(s => s.Artist)
                .Include(s => s.Album)  
                .OrderByDescending(s => s.Reviews.Average(r => (double?)r.Rating) ?? 0)
                .Take(10)
                .ToListAsync();
        }

        public async Task<IEnumerable<Song>> SearchSongsAsync(string query)
        {
            return await _repository.SongRepository
                .FindByCondition(a => a.Title.Contains(query))
                .Include(a => a.Artist)
                .ToListAsync();
        }

    }
}
