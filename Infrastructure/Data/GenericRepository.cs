using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        public void Add(T entity)
        {
            context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            //var local = context.Set<T>().Local.FirstOrDefault(entry => entry.Id == entity.Id);
            //if (local != null)
            //{
            //    // Detach the already tracked entity
            //    context.Entry(local).State = EntityState.Detached;
            //}

            //context.Set<T>().Attach(entity);
            //context.Entry(entity).State = EntityState.Modified;

            context.Update(entity); // this will handle the navigation collections correctly
        }

        public void Remove(T entity)
        {
            context.Set<T>().Remove(entity);
        }
        public bool IsExists(int id)
        {
            return context.Set<T>().Any(x => x.Id == id);
        }
    }
}
