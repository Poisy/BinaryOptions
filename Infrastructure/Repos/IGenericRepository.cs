using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repos
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(Guid id);
        
        Task<IEnumerable<T>> GetAll();
        
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression);
        
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
        
        Task Add(T entity);
        
        void Remove(T entity);
        
        void Update(Guid id, T entity);

        Task Complete();
    }
}