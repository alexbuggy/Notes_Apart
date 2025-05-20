using Microsoft.AspNetCore.Mvc;
using NotesApart.Services.Interfaces;
using NotesApart.ViewModels;
using NotesApart.Models;
using NotesApart.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace NotesApart.Controllers
{
    public class AlbumController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly IReviewService _reviewService;
        private readonly IFavouriteAlbumService _favouriteAlbumService;
        private readonly ILikedReviewService _likedReviewService;
        private readonly UserManager<User> _userManager;

        public AlbumController(IAlbumService albumService, IReviewService reviewService, IFavouriteAlbumService favouriteAlbumService, ILikedReviewService likedReviewService, UserManager<User> userManager)
        {
            _albumService = albumService;
            _reviewService = reviewService;
            _favouriteAlbumService = favouriteAlbumService;
            _likedReviewService = likedReviewService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var isFavorited = userId != null && _favouriteAlbumService.IsFavorited(userId, id);
            var likedReviews = userId != null
                ? await _likedReviewService.GetLikedReviewsByUserIdAsync(userId)
                : new List<LikedReview>();

            var album = await _albumService.GetAlbumByIdAsync(id);
            if (album == null) return NotFound();

            var reviews = await _reviewService.GetReviewsByAlbumIdAsync(id);

            var viewModel = new AlbumDetailsViewModel
            {
                Album = album,
                Reviews = reviews?.ToList() ?? new List<Review>(),
                IsFavorited = isFavorited,
                LikedReviewIds = likedReviews.Select(lr => lr.ReviewId).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateReview(int albumId, string content, int rating)
        {
            if (string.IsNullOrWhiteSpace(content) || rating <= 0)
            {
                TempData["Error"] = "Invalid review data!";
                return RedirectToAction("Details", new { id = albumId });
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var review = new Review
            {
                AlbumId = albumId,
                Rating = rating,
                Content = content,
                UserId = userId
            };

            _reviewService.CreateReview(review);

            return RedirectToAction("Details", new { id = albumId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int reviewId, int albumId)
        {
            var userId = _userManager.GetUserId(User);
            var review = await _reviewService.GetReviewByIdAsync(reviewId);

            if (review == null || (review.UserId != userId && !User.IsInRole("Admin")))
                return Forbid();

            await _reviewService.DeleteReviewAsync(reviewId);
            return RedirectToAction("Details", new { id = albumId });
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddFavouriteAlbum(int albumId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            if (_favouriteAlbumService.IsFavorited(userId, albumId))
            {
                _favouriteAlbumService.RemoveFavouriteAlbum(userId, albumId);
            }
            else
            {
                _favouriteAlbumService.AddFavouriteAlbum(userId, albumId);
            }

            return RedirectToAction("Details", new { id = albumId });
        }
    }
}