using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using NotesApart.Repositories;

namespace NotesApart.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IRepositoryWrapper _repository;

        public ArtistService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _repository.ArtistRepository.FindAll().ToListAsync();
        }

        public async Task<Artist> GetArtistByIdAsync(int artistId)
        {
            return await _repository.ArtistRepository.FindByCondition(a => a.ArtistId == artistId)
                .Include(a => a.Albums)
                    .ThenInclude(album => album.Reviews)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Album>> GetAllAlbumsByArtistAsync(int artistId)
        {
            return await _repository.AlbumRepository
                .FindByCondition(a => a.ArtistId == artistId)
                .ToListAsync();
        }
        public async Task<double> GetAverageAlbumRatingByArtistIdAsync(int artistId)
        {
            var albums = _repository.AlbumRepository.FindByCondition(a => a.ArtistId == artistId);
            var albumIds = albums.Select(a => a.AlbumId).ToList();
            var reviews = _repository.ReviewRepository.FindByCondition(r => r.AlbumId != null && albumIds.Contains((int)r.AlbumId));
            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            return await Task.FromResult(averageRating);
        }

        public async Task<double> GetAverageRatingForArtistAsync(int artistId)
        {
            var albums = await _repository.AlbumRepository
                .FindByCondition(a => a.ArtistId == artistId)
                .Include(a => a.Reviews)
                .ToListAsync();

            if (!albums.Any())
                return 0;

            var allRatings = albums.SelectMany(a => a.Reviews).Select(r => (double?)r.Rating).ToList();

            return allRatings.Any() ? allRatings.Average().Value : 0;
        }

        public async Task<List<Artist>> FetchArtistsByNameAsync(string name)
        {
            return await _repository.ArtistRepository
                .FindByCondition(a => a.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<Artist> GetArtistBySpotifyID(string spotifyId)
        {
            return await _repository.ArtistRepository
             .FindByCondition(a => a.SpotifyArtistId == spotifyId)
             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Artist>> SearchArtistsAsync(string query)
        {
            return await _repository.ArtistRepository
            .FindByCondition(a => a.Name.Contains(query))
            .ToListAsync();
        }
    }
}