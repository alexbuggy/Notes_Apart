using Microsoft.AspNetCore.Mvc;
using NotesApart.Services.Interfaces;

public class ArtistController : Controller
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    public async Task<IActionResult> Index()
    {
        var artists = await _artistService.GetAllArtistsAsync();
        return View(artists);
    }

    public async Task<IActionResult> Details(int id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);
        if (artist == null)
            return NotFound();

        var averageRating = await _artistService.GetAverageRatingForArtistAsync(id);
        ViewBag.AverageRating = averageRating;

        return View(artist);
    }
}
