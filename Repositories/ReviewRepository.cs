using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class ReviewRepository : RepositoryBase<Review>, IReviewRepository
    {
        public ReviewRepository(NotesApartDbContext notesapartContext) : base(notesapartContext)
        {
        }
    }
}
