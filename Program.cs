using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Repositories;
using System;
using NotesApart.Services.Interfaces;
using NotesApart.Services;
using NotesApart.Integrations.Spotify;
using NotesApart.Integrations.Spotify.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using NotesApart.Services.Auth;





var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<SpotifyClient>(); // singleton for auth reuse
builder.Services.AddScoped<ISpotifyService, SpotifyService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IFavouriteAlbumRepository,FavouriteAlbumRepository>();
builder.Services.AddScoped<IFavouriteSongRepository, FavouriteSongRepository>();
builder.Services.AddScoped<ILikedReviewRepository, LikedReviewRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IFavouriteAlbumService, FavouriteAlbumService>();
builder.Services.AddScoped<IFavouriteSongService, FavouriteSongService>();
builder.Services.AddScoped<ILikedReviewService, LikedReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();




builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<NotesApartDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddDbContext<NotesApartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var user = await userManager.FindByEmailAsync("admin@gmail.com");

    if (user != null)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var result = await userManager.ResetPasswordAsync(user, token, "Admin123!");

        if (result.Succeeded)
            Console.WriteLine("Admin password reset to Admin123!");
        else
            foreach (var err in result.Errors)
                Console.WriteLine(err.Description);
    }
}

using (var scope = app.Services.CreateScope())
{
    var spotifyService = scope.ServiceProvider.GetRequiredService<ISpotifyService>();
    await spotifyService.SeedInitialDataAsync();
}

using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    await authService.SeedRolesAndAdminAsync();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapRazorPages();
app.Run();
