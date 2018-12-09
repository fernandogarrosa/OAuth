using Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Fields

        protected IDbContext context;

        #endregion Fields

        #region IGenericRepository

        public Repository(IDbContext context)
        {
            this.context = context;
        }

        public virtual T Add(T entity)
        {
            context.Set<T>().Add(entity);
            context.SaveChanges();

            return entity;
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public int Count() => context.Set<T>().Count();

        public async Task<int> CountAsync() => await context.Set<T>().CountAsync();

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
            context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync();
        }

        public void Delete(params object[] id)
        {
            T entityToDelete = context.Set<T>().Find(id);

            if (entityToDelete != null)
                context.Set<T>().Remove(entityToDelete);

            context.SaveChanges();
        }

        public virtual async Task<int> DeleteAsync(params object[] id)
        {
            T entityToDelete = context.Set<T>().Find(id);

            if (entityToDelete != null)
                context.Set<T>().Remove(entityToDelete);

            return await context.SaveChangesAsync();
        }

        public virtual T Find(Expression<Func<T, bool>> match) => context.Set<T>().SingleOrDefault(match);

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match) => await context.Set<T>().SingleOrDefaultAsync(match);

        public ICollection<T> FindAll(Expression<Func<T, bool>> match) => context.Set<T>().Where(match).ToList();

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match) => await context.Set<T>().Where(match).ToListAsync();

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) => context.Set<T>().Where(predicate);

        public virtual async Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate) => await context.Set<T>().Where(predicate).ToListAsync();

        public virtual T Get(int id) => context.Set<T>().Find(id);

        public virtual async Task<T> GetAsync(int id) => await context.Set<T>().FindAsync(id);

        public IQueryable<T> GetTable() => context.Set<T>();

        public virtual async Task<ICollection<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

        public virtual void Save() => context.SaveChanges();

        public async virtual Task<int> SaveAsync() => await context.SaveChangesAsync();

        public virtual T Update(T entity, params object[] key)
        {
            if (entity == null)
                return null;

            T exist = context.Set<T>().Find(key);

            if (exist != null)
            {
                context.Entry(exist).CurrentValues.SetValues(entity);
                context.SaveChanges();
            }

            return exist;
        }

        public virtual async Task<T> UpdateAsync(T entity, params object[] key)
        {
            if (entity == null)
                return null;

            T exist = await context.Set<T>().FindAsync(key);

            if (exist != null)
            {
                context.Entry(exist).CurrentValues.SetValues(entity);
                await context.SaveChangesAsync();
            }

            return exist;
        }

        #endregion IGenericRepository

        #region Dispose Pattern

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                context.Dispose();

            disposed = true;
        }

        #endregion Dispose Pattern
    }
}
