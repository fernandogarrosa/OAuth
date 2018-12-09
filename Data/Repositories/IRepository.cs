using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);

        Task<T> AddAsync(T entity);

        int Count();

        Task<int> CountAsync();

        void Delete(T entity);

        Task<int> DeleteAsync(T entity);

        void Delete(params object[] id);

        Task<int> DeleteAsync(params object[] id);

        T Find(Expression<Func<T, bool>> match);

        Task<T> FindAsync(Expression<Func<T, bool>> match);

        ICollection<T> FindAll(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> FindByAsyn(Expression<Func<T, bool>> predicate);

        T Get(int id);

        Task<T> GetAsync(int id);

        IQueryable<T> GetTable();

        Task<ICollection<T>> GetAllAsync();

        void Save();

        Task<int> SaveAsync();

        T Update(T entity, params object[] key);

        Task<T> UpdateAsync(T entity, params object[] key);

        void Dispose();
    }
}
