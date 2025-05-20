using Microsoft.EntityFrameworkCore;
using NotesApart.Models;
using NotesApart.Repositories.Interfaces;
using System.Linq.Expressions;

namespace NotesApart.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected NotesApartDbContext NotesApartDbContext { get; set; }

        public RepositoryBase(NotesApartDbContext notesapartContext)
        {
            this.NotesApartDbContext = notesapartContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.NotesApartDbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.NotesApartDbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            this.NotesApartDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.NotesApartDbContext.Set<T>().Update(entity);
            NotesApartDbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            this.NotesApartDbContext.Set<T>().Remove(entity);
            NotesApartDbContext.SaveChanges();
        }
    }
}
