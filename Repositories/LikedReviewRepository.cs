using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class LikedReviewRepository : RepositoryBase<LikedReview>, ILikedReviewRepository
    {
        public LikedReviewRepository(NotesApartDbContext notesapartContext) : base(notesapartContext)
        {
        }
    }
}
