using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        
        Task<IEnumerable<T>> GetAllAsync();
        
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
        
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        
        Task AddAsync(T entity);
        
        void Remove(T entity);
        
        void Update(Guid id, T entity);

        Task CompleteAsync();
    }
}