using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApart.Models;
using NotesApart.Services.Interfaces;
using System.Security.Claims;

namespace NotesApart.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFavouriteSongService _favoriteSongService;
        private readonly IFavouriteAlbumService _favoriteAlbumService;
        private readonly IReviewService _reviewService;

        public UserController(IUserService userService, IFavouriteSongService favoriteSongService,
                              IReviewService reviewService, IFavouriteAlbumService favoriteAlbumService)
        {
            _userService = userService;
            _favoriteAlbumService = favoriteAlbumService;
            _favoriteSongService = favoriteSongService;
            _reviewService = reviewService;

        }


        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserByIdAsync(userId);

            ViewData["IsPublicProfile"] = false;


            return View(user);
        }

        [Route("User/PublicProfile/{userId}")]
        public async Task<IActionResult> PublicProfile(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userService.GetUserByIdAsync(userId);
            if (currentUserId == userId)
            {
                ViewData["IsPublicProfile"] = false;
            }
            else
            {
                ViewData["IsPublicProfile"] = true;
            }
            return View("Profile", user); ;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(User updatedUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userService.UpdateProfileAsync(userId, updatedUser.UserName, updatedUser.ProfileImg);
            return RedirectToAction("Profile");
        }
    }
}
