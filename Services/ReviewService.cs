using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class ReviewService:IReviewService
    {
        private readonly IRepositoryWrapper _repository;

        public ReviewService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _repository.ReviewRepository.FindAll()
                .Include(r => r.Album).ThenInclude(a => a.Artist)  
                .Include(r => r.Song).ThenInclude(s => s.Artist)   
                .Include(r => r.Song).ThenInclude(s => s.Album)    
                .ToListAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int reviewId)
        {
            return await _repository.ReviewRepository.FindByCondition(r => r.ReviewId == reviewId)
                .Include(r => r.LikedByUsers)
                .Include(r => r.Album).ThenInclude(a => a.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Album)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Review>> GetTopRatedReviewsAsync()
        {
            return await _repository.ReviewRepository.FindAll()
                 .Include(r => r.LikedByUsers)
                 .Include(r => r.Album).ThenInclude(a => a.Artist)
                 .Include(r => r.Song).ThenInclude(s => s.Artist)
                 .Include(r => r.Song).ThenInclude(s => s.Album)
                 .OrderByDescending(r => r.LikedByUsers.Count) 
                    .Take(3)
                    .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByAlbumIdAsync(int albumId)
        {
            return await _repository.ReviewRepository.FindByCondition(r => r.AlbumId == albumId)
                .Include(r => r.LikedByUsers)
                .Include(r => r.Album).ThenInclude(a => a.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Album)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsBySongIdAsync(int songId)
        {
            return await _repository.ReviewRepository.FindByCondition(r => r.SongId == songId)
                .Include(r => r.LikedByUsers)
                .Include(r => r.Album).ThenInclude(a => a.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Album)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId)
        {
            return await _repository.ReviewRepository
                .FindByCondition(r => r.UserId == userId)
                .Include(r => r.LikedByUsers)
                .Include(r => r.Album).ThenInclude(a => a.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Artist)
                .Include(r => r.Song).ThenInclude(s => s.Album)
                .ToListAsync();
        }

        public void CreateReview(Review review)
        {
            _repository.ReviewRepository.Create(review);
            _repository.Save();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _repository.ReviewRepository
        .FindByCondition(r => r.ReviewId == id)
        .Include(r => r.LikedByUsers)
        .FirstOrDefaultAsync();

            if (review == null) return;

            if (review.LikedByUsers != null && review.LikedByUsers.Any())
            {
                foreach (var liked in review.LikedByUsers.ToList())
                {
                    _repository.LikedReviewRepository.Delete(liked);
                }
            }

            _repository.ReviewRepository.Delete(review);
             _repository.Save();
        }
        public void  AddReview(String userId, int? albumId, int? songId, int rating, string content)
        {
            var review = new Review
            {
                UserId = userId,
                AlbumId = albumId,
                SongId = songId,
                Rating = rating,
                Content = content
            };

            _repository.ReviewRepository.Create(review);
            _repository.Save();
        }
    }
}
