using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class LikedReviewService:ILikedReviewService
    {
        private readonly IRepositoryWrapper _repository;

        public LikedReviewService(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<LikedReview>> GetLikedReviewsByUserIdAsync(string userId)
        {
            return await _repository.LikedReviewRepository
                .FindByCondition(lr => lr.UserId == userId)
                .Include(lr => lr.Review)
                .ToListAsync();
        }

        public void LikeReview(LikedReview likedReview)
        {
            _repository.LikedReviewRepository.Create(likedReview);
            _repository.Save();
        }

        public async Task UnlikeReviewAsync(string userId, int reviewId)
        {
            var likedReview = await _repository.LikedReviewRepository
                .FindByCondition(lr => lr.UserId == userId && lr.ReviewId == reviewId)
                .FirstOrDefaultAsync();

            if (likedReview != null)
            {
                _repository.LikedReviewRepository.Delete(likedReview);
                _repository.Save();
            }
        }
    }
}
