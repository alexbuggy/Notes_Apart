using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class SongRepository : RepositoryBase<Song>, ISongRepository
    {
        public SongRepository(NotesApartDbContext notesapartContext) : base(notesapartContext)
        {
        }
    }
}
