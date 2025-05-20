using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly UserManager<User> _userManager;

        public UserService(IRepositoryWrapper repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _repo.UserRepository
               .FindByCondition(u => u.Id == id)
               .Include(u => u.FavoriteSongs)
                   .ThenInclude(fg => fg.Song)
               .Include(u => u.Reviews)
               .ThenInclude(fg => fg.Song)
               .Include(u => u.Reviews)
               .ThenInclude(fg => fg.Album)
               .Include(u => u.FavoriteAlbums)
               .ThenInclude(gr => gr.Album)
               .FirstOrDefaultAsync();
        }

        public async Task UpdateProfileAsync(string id, string newUsername, string newImg)
        {
            var user = _repo.UserRepository.FindByCondition(u => u.Id == id).FirstOrDefault();
            if (user == null) return;

            user.UserName = newUsername;
            user.ProfileImg = newImg;
            _repo.UserRepository.Update(user);
        }
    }
}
