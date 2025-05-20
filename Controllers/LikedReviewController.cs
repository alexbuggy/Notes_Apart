using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApart.Models;
using NotesApart.Services.Interfaces;
using System.Security.Claims;

namespace NotesApart.Controllers
{
    public class LikedReviewController : Controller
    {
        private readonly ILikedReviewService _likedReviewService;

        public LikedReviewController(ILikedReviewService likedReviewService)
        {
            _likedReviewService = likedReviewService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ToggleLike(int reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var likedReviews = await _likedReviewService.GetLikedReviewsByUserIdAsync(userId);
            var alreadyLiked = likedReviews.Any(lr => lr.ReviewId == reviewId);

            if (alreadyLiked)
            {
                await _likedReviewService.UnlikeReviewAsync(userId, reviewId);
            }
            else
            {
                _likedReviewService.LikeReview(new LikedReview { UserId = userId, ReviewId = reviewId });
            }

            return Redirect(Request.Headers["Referer"].ToString()); // Go back to previous page
        }
    }
}
