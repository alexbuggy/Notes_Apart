using NotesApart.Models;
using NotesApart.Repositories.Interfaces;

namespace NotesApart.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(NotesApartDbContext notesApartDbContext) : base(notesApartDbContext)
        {
        }
    }
}
