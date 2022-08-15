using System;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Repos;

namespace Infrastructure.Services
{
    public class UserService
    {
        private readonly IGenericRepository<ApplicationUser> _repo;

        public UserService(IGenericRepository<ApplicationUser> repo)
        {
            _repo = repo;
        }

        public async Task<ApplicationUser> GetById(Guid id)
        {
            return await _repo.GetById(id);
        }
        
        
    }
}