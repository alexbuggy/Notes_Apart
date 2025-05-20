using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NotesApart.Models;
using NotesApart.Services.Interfaces;
using System.Threading.Tasks;

namespace NotesApart.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           RoleManager<IdentityRole> roleManager,
                           ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterAsync(string email, string password)
        {
            var user = new User { Email = email, UserName = email };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "User");

          
            await _signInManager.SignInAsync(user, isPersistent: false);

            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, "User not found");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded) return (true, null);
            return (false, "Invalid login attempt");
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task SeedRolesAndAdminAsync()
        {
            var roles = new[] { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new User { Email = adminEmail, UserName = "AdminUser" };
                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    _logger.LogInformation("Seeded admin user.");
                }
            }
        }
    }
}