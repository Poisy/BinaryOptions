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
        //=============================================================================================
        private readonly BinaryOptionsDbContext _db;

        
        //=============================================================================================
        public GenericRepository(BinaryOptionsDbContext db)
        {
            _db = db;
        }
        
        
        //=============================================================================================
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        //=============================================================================================
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        //=============================================================================================
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().Where(expression).ToListAsync();
        }

        //=============================================================================================
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _db.Set<T>().FirstOrDefaultAsync(expression);
        }

        //=============================================================================================
        public async Task AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
        }

        //=============================================================================================
        public void Remove(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        //=============================================================================================
        public void Update(Guid id, T entity)
        {
            _db.Set<T>().Update(entity);
        }

        //=============================================================================================
        public async Task CompleteAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}