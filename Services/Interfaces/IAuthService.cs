namespace NotesApart.Services.Interfaces
{
    public interface IAuthService
    {
        Task<(bool Success, string ErrorMessage)> RegisterAsync(string email, string password);
        Task<(bool Success, string ErrorMessage)> LoginAsync(string email, string password, bool rememberMe);
        Task LogoutAsync();
        Task SeedRolesAndAdminAsync();
    }
}
