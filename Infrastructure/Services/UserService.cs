using System;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Repos;

namespace Infrastructure.Services
{
    public class UserService
    {
        //=============================================================================================
        private readonly IGenericRepository<ApplicationUser> _repo;

        
        //=============================================================================================
        public UserService(IGenericRepository<ApplicationUser> repo)
        {
            _repo = repo;
        }

        
        //=============================================================================================
        public async Task<ApplicationUser> GetById(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        //=============================================================================================
        public async Task<ApplicationUser> GetByUsername(string username)
        {
            return await _repo.FirstOrDefaultAsync(user => user.UserName == username);
        }

        //=============================================================================================
        public async Task<bool> ExistByEmail(string email)
        {
            var user = await _repo.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;
        }

        public async Task<bool> RemoveFromBalance(ApplicationUser user, double valueToRemove)
        {
            if (user.Balance - valueToRemove < 0)
            {
                return false;
            }

            user.Balance -= valueToRemove;
            
            _repo.Update(new Guid(user.Id), user);
            await _repo.CompleteAsync();

            return true;
        }
    }
}