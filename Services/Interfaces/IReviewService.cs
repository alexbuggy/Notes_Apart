using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task<IEnumerable<Review>> GetTopRatedReviewsAsync();
        Task<IEnumerable<Review>> GetReviewsByAlbumIdAsync(int albumId);
        Task<IEnumerable<Review>> GetReviewsBySongIdAsync(int songId);
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(string userId);
        void AddReview(string userId, int? albumId, int? songId, int rating, string content);
        void CreateReview(Review review);
        Task DeleteReviewAsync(int id);
    }
}
