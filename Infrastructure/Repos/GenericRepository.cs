using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BinaryOptionsDbContext _db;

        public GenericRepository(BinaryOptionsDbContext db)
        {
            _db = db;
        }
        
        public async Task<T> GetById(Guid id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _db.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().FirstAsync(expression);
        }

        public async Task Add(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public void Update(Guid id, T entity)
        {
            _db.Set<T>().Update(entity);
        }

        public async Task Complete()
        {
            await _db.SaveChangesAsync();
        }
    }
}