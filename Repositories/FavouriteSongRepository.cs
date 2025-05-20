using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class FavouriteSongRepository : RepositoryBase<FavouriteSong>, IFavouriteSongRepository
    {
        public FavouriteSongRepository(NotesApartDbContext notesapartContext) : base(notesapartContext)
        {
        }
    }
}
