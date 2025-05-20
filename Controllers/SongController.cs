using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesApart.Models;
using NotesApart.Services;
using NotesApart.Services.Interfaces;
using NotesApart.ViewModels;

namespace NotesApart.Controllers
{
    public class SongController : Controller
    {
        private readonly ISongService _songService;
        private readonly IReviewService _reviewService;
        private readonly IFavouriteSongService _favouriteSongService;
        private readonly ILikedReviewService _likedReviewService;
        private readonly UserManager<User> _userManager;

        public SongController(ISongService songService, IReviewService reviewService, IFavouriteSongService favouriteSongService,ILikedReviewService likedReviewService,UserManager<User> userManager)
        {
            _songService = songService;
            _reviewService = reviewService;
            _favouriteSongService = favouriteSongService;
            _likedReviewService = likedReviewService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var isFavorited = userId != null && _favouriteSongService.IsFavorited(userId, id);
            var likedReviews = userId != null
                ? await _likedReviewService.GetLikedReviewsByUserIdAsync(userId)
                : new List<LikedReview>();

            var song = await _songService.GetSongByIdAsync(id);
            if (song == null)
                return NotFound();

            var reviews = await _reviewService.GetReviewsBySongIdAsync(id);

            var viewModel = new SongDetailsViewModel
            {
                Song = song,
                Reviews = reviews.ToList(),
                IsFavorited = isFavorited,
                LikedReviewIds = likedReviews.Select(lr => lr.ReviewId).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateReview(int songId, int rating, string content)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null || string.IsNullOrWhiteSpace(content) || rating <= 0)
            {
                TempData["Error"] = "Invalid review data!";
                return RedirectToAction("Details", new { id = songId });
            }

            var review = new Review
            {
                SongId = songId,
                Rating = rating,
                Content = content,
                UserId = userId
            };

            _reviewService.CreateReview(review);

            return RedirectToAction("Details", new { id = songId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int reviewId, int songId)
        {
            var userId = _userManager.GetUserId(User);
            var review = await _reviewService.GetReviewByIdAsync(reviewId);


            if (review == null || (review.UserId != userId && !User.IsInRole("Admin")))
                return Forbid();

            await _reviewService.DeleteReviewAsync(reviewId);
            return RedirectToAction("Details", new { id = songId });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddFavouriteSong(int songId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            if (_favouriteSongService.IsFavorited(userId, songId))
            {
                _favouriteSongService.RemoveFavouriteSong(userId, songId);
            }
            else
            {
                _favouriteSongService.AddFavouriteSong(userId, songId);
            }

            return RedirectToAction("Details", new { id = songId });
        }
    }
}
