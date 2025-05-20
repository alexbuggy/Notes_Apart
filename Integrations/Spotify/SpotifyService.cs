using NotesApart.Integrations.Spotify.Interfaces;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using NotesApart.Services;
using NotesApart.Services.Interfaces;

namespace NotesApart.Integrations.Spotify
{
    public class SpotifyService:ISpotifyService
    {
        private readonly SpotifyClient _client;
        private readonly IRepositoryWrapper _repository;
        private readonly IArtistService _artistService;
        private readonly IAlbumService _albumService;
        private readonly ISongService _songService;

        public SpotifyService(SpotifyClient client, IRepositoryWrapper repository,IArtistService artistService, IAlbumService albumService, ISongService songService)
        {
            _client = client;
            _repository = repository;
            _artistService = artistService;
            _albumService = albumService;
            _songService = songService;
        }

        public async Task SeedInitialDataAsync()
        {
            string[] popularArtistNames = new[] { "Radiohead", "Kendrick Lamar", "Taylor Swift", "Drake", "Coldplay", "John Mayer", "Travis Scott", "MF DOOM", "King Gizzard & The Lizard Wizard", "Metallica"
            ,"The Prodigy","Alternosfera","Rush","Alice In Chains","King Crimson","Led Zeppelin","Slowthai","Skepta","Gorillaz","Florin Salam"};

            foreach (var name in popularArtistNames)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                try
                {
                    await FetchArtistsByNameAsync(name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Failed to fetch artist '{name}': {ex.Message}");
                }
            }

            _repository.Save();
        }

        public async Task<List<Artist>> FetchArtistsByNameAsync(string name)
        {
            var json = await _client.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(name)}&type=artist&limit=10");
            var items = json.RootElement.GetProperty("artists").GetProperty("items");

            var artists = new List<Artist>();

            foreach (var artistJson in items.EnumerateArray())
            {
                var spotifyId = artistJson.GetProperty("id").GetString();

                if (await _artistService.GetArtistBySpotifyID(spotifyId) != null)
                    continue;

                var artist = new Artist
                {
                    Name = artistJson.GetProperty("name").GetString(),
                    SpotifyArtistId = spotifyId,
                    ImageUrl = artistJson.GetProperty("images").EnumerateArray().FirstOrDefault().GetProperty("url").GetString()
                };

                _repository.ArtistRepository.Create(artist);
                _repository.Save();
                await FetchAlbumsByArtistIdAsync(spotifyId, artist);
                artists.Add(artist);
            }

            return artists;
        }

        private async Task FetchAlbumsByArtistIdAsync(string artistSpotifyId, Artist artist)
        {
            var json = await _client.GetAsync($"https://api.spotify.com/v1/artists/{artistSpotifyId}/albums?include_groups=album&limit=50");
            foreach (var item in json.RootElement.GetProperty("items").EnumerateArray())
            {
                var album = new Album
                {
                    Title = item.GetProperty("name").GetString(),
                    CoverImageUrl = item.GetProperty("images").EnumerateArray().FirstOrDefault().GetProperty("url").GetString(),
                    SpotifyAlbumId = item.GetProperty("id").GetString(),
                    Artist = artist
                };


                var existingAlbum = await _albumService.GetAlbumBySpotifyID(album.SpotifyAlbumId);

                if (existingAlbum != null)
                    return;

                _repository.AlbumRepository.Create(album);
                _repository.Save();
                await FetchTracksForAlbumAsync(album);
            }
        }

        private async Task FetchTracksForAlbumAsync(Album album)
        {
            var json = await _client.GetAsync($"https://api.spotify.com/v1/albums/{album.SpotifyAlbumId}/tracks");
            int trackNumber = 1;
            foreach (var track in json.RootElement.GetProperty("items").EnumerateArray())
            {
                var song = new Song
                {
                    Title = track.GetProperty("name").GetString(),
                    SpotifySongId = track.GetProperty("id").GetString(),
                    TrackNumber = trackNumber++,
                    Artist = album.Artist,
                    Album = album
                };

                var existingSong = await _songService.GetSongBySpotifyID(song.SpotifySongId);


                if (existingSong != null)
                    return;

                _repository.SongRepository.Create(song);
                _repository.Save();
            }
        }

        public async Task<Album> FetchAlbumByNameAsync(string name)
        {
            var json = await _client.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(name)}&type=album&limit=1");
            var albumJson = json.RootElement.GetProperty("albums").GetProperty("items")[0];

            var albumId = albumJson.GetProperty("id").GetString();

            var existingAlbum = await _albumService.GetAlbumBySpotifyID(albumId);
            if (existingAlbum != null)
                return existingAlbum;

            var artistJson = albumJson.GetProperty("artists")[0];
            var artistName = artistJson.GetProperty("name").GetString();

            var artistList = await FetchArtistsByNameAsync(artistName);
            var artist = artistList.FirstOrDefault(); 

            if (artist == null) throw new Exception("No artist found."); 

            var album = new Album
            {
                Title = albumJson.GetProperty("name").GetString(),
                SpotifyAlbumId = albumId,
                CoverImageUrl = albumJson.GetProperty("images").EnumerateArray().FirstOrDefault().GetProperty("url").GetString(),
                Artist = artist
            };

            _repository.AlbumRepository.Create(album);
            _repository.Save();
            await FetchTracksForAlbumAsync(album);

            return album;
        }




        public async Task<Song> FetchSongByNameAsync(string name)
        {
            var json = await _client.GetAsync($"https://api.spotify.com/v1/search?q={Uri.EscapeDataString(name)}&type=track&limit=1");
            var trackJson = json.RootElement.GetProperty("tracks").GetProperty("items")[0];

            var spotifySongId = trackJson.GetProperty("id").GetString();

            var existingSong = await _songService.GetSongBySpotifyID(spotifySongId);
            if (existingSong != null)
                return existingSong;


            var artistJson = trackJson.GetProperty("artists")[0];
            var artistName = artistJson.GetProperty("name").GetString();
            var artistList = await FetchArtistsByNameAsync(artistName);
            var artist = artistList.FirstOrDefault();

            if (artist == null) throw new Exception("No artist found.");


            var albumJson = trackJson.GetProperty("album");
            var spotifyAlbumId = albumJson.GetProperty("id").GetString();

            var existingAlbum = await _albumService.GetAlbumBySpotifyID(spotifyAlbumId);
            Album album;

            if (existingAlbum != null)
            {
                album = existingAlbum;
            }
            else
            {
                album = new Album
                {
                    Title = albumJson.GetProperty("name").GetString(),
                    SpotifyAlbumId = spotifyAlbumId,
                    CoverImageUrl = albumJson.GetProperty("images").EnumerateArray().FirstOrDefault().GetProperty("url").GetString(),
                    Artist = artist
                };

                _repository.AlbumRepository.Create(album);
            }

            var song = new Song
            {
                Title = trackJson.GetProperty("name").GetString(),
                SpotifySongId = spotifySongId,
                TrackNumber = trackJson.GetProperty("track_number").GetInt32(),
                Artist = artist,
                Album = album
            };

            _repository.SongRepository.Create(song);
            _repository.Save();
            return song;
        }
    }
}
