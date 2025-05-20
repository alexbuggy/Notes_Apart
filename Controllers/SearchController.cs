using Microsoft.AspNetCore.Mvc;
using NotesApart.Integrations.Spotify.Interfaces;
using NotesApart.Services.Interfaces;

namespace NotesApart.Controllers
{
    public class SearchController : Controller
    {
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;
        private readonly IArtistService _artistService;
        private readonly ISpotifyService _spotifyService;

        public SearchController(IAlbumService albumService, ISongService songService, IArtistService artistService, ISpotifyService spotifyService)
        {
            _albumService = albumService;
            _songService = songService;
            _artistService = artistService;
            _spotifyService = spotifyService;
        }

        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return View(new NotesApart.ViewModels.SearchViewModel());

            var albums = await _albumService.SearchAlbumsAsync(query);
            if (!albums.Any())
            {
                try
                {
                   await _spotifyService.FetchArtistsByNameAsync(query);
                    albums = await _albumService.SearchAlbumsAsync(query); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Spotify Fallback] Album fetch failed: {ex.Message}");
                }
            }

            var songs = await _songService.SearchSongsAsync(query);
            if (!songs.Any())
            {
                try
                {
                    await _spotifyService.FetchSongByNameAsync(query);
                    songs = await _songService.SearchSongsAsync(query);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Spotify Fallback] Song fetch failed: {ex.Message}");
                }
            }

            var artists = await _artistService.SearchArtistsAsync(query);
            if (!artists.Any())
            {
                try
                {
                    await _spotifyService.FetchArtistsByNameAsync(query);
                    artists = await _artistService.SearchArtistsAsync(query);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Spotify Fallback] Artist fetch failed: {ex.Message}");
                }
            }


            var viewModel = new NotesApart.ViewModels.SearchViewModel
            {
                Query = query,
                Albums = albums.ToList(),
                Songs = songs.ToList(),
                Artists = artists.ToList()
            };

            return View(viewModel);
        }



    }
}
