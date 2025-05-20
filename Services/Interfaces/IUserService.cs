using NotesApart.Models;

namespace NotesApart.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(string id);
        Task UpdateProfileAsync(string id, string newUsername, string newImg);
    }
}
