using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface ILikedReviewService
    {
        Task<IEnumerable<LikedReview>> GetLikedReviewsByUserIdAsync(string userId);
        void LikeReview(LikedReview likedReview);
        Task UnlikeReviewAsync(string userId, int reviewId);
    }
}
