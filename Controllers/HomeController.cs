using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NotesApart.Models;
using NotesApart.ViewModels;
using NotesApart.Services.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace NotesApart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IReviewService _reviewService;
        private readonly ILikedReviewService _likedReviewService;
        private readonly UserManager<User> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            IAlbumService albumService
            ,ISongService songService,
            IReviewService reviewService,
             ILikedReviewService likedReviewService,
             UserManager<User> userManager)
        {
            _logger = logger;
            _albumService = albumService;
            _songService = songService;
            _reviewService = reviewService;
            _likedReviewService = likedReviewService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var topAlbums = await _albumService.GetTopRatedAlbumsAsync();
            var topSongs = await _songService.GetTopRatedSongsAsync();
            var topReviews = await _reviewService.GetTopRatedReviewsAsync();


            var userId = _userManager.GetUserId(User);
            var likedReviews = userId != null
                ? await _likedReviewService.GetLikedReviewsByUserIdAsync(userId)
                : new List<LikedReview>();

            var viewModel = new HomePageViewModel
            {
                TopRatedAlbums = topAlbums,
                TopRatedSongs = topSongs,
                TopReviews = topReviews,
                LikedReviewIds = likedReviews.Select(lr => lr.ReviewId).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();  
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
