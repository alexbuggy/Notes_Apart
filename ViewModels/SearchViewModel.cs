using NotesApart.Models;

namespace NotesApart.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public List<Album> Albums { get; set; }
        public List<Song> Songs { get; set; }
        public List<Artist> Artists { get; set; }
    }
}
