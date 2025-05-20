using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class FavouriteAlbumRepository : RepositoryBase<FavouriteAlbum>, IFavouriteAlbumRepository
    {
        public FavouriteAlbumRepository(NotesApartDbContext notesapartContext) : base(notesapartContext)
        {
        }
    }
}
