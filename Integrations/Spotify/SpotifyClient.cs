using System.Net.Http.Headers;
using System.Text.Json;
namespace NotesApart.Integrations.Spotify
{
    public class SpotifyClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private string _accessToken;
        private DateTime _tokenExpiry;

        public SpotifyClient(IConfiguration config)
        {
            _clientId = config["Spotify:ClientId"];
            _clientSecret = config["Spotify:ClientSecret"];
            _httpClient = new HttpClient();
        }

        private async Task AuthenticateAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
                return;

            var auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials"
            });

            var response = await _httpClient.SendAsync(request);
            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            _accessToken = json.RootElement.GetProperty("access_token").GetString();
            _tokenExpiry = DateTime.UtcNow.AddSeconds(json.RootElement.GetProperty("expires_in").GetInt32() - 60);
        }

        public async Task<JsonDocument> GetAsync(string url)
        {
            await AuthenticateAsync();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        }
    }
}
