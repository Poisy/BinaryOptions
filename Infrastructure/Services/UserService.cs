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
        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _repo.GetByIdAsync(id);
        }

        
        //=============================================================================================
        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            return await _repo.FirstOrDefaultAsync(user => user.UserName == username);
        }

        
        //=============================================================================================
        public async Task<bool> ExistByEmail(string email)
        {
            var user = await _repo.FirstOrDefaultAsync(user => user.Email == email);

            return user != null;
        }

        
        //=============================================================================================
        public async Task<ApplicationUser> GetAdminAsync()
            => await _repo.FirstOrDefaultAsync(user => user.UserName == "Admin");

        
        //=============================================================================================
        public async Task<bool> CanSystemPayAsync(double payout)
            => (await GetAdminAsync()).Balance > payout;
        
        
        //=============================================================================================
        public bool CanUserPay(ApplicationUser user, double value)
            => user.Balance - value >= 0;

        
        //=============================================================================================
        public async Task TransferMoneyFromUserToSystemAsync(ApplicationUser user, double value)
        {
            var admin = await GetAdminAsync();
            
            user.Balance -= value;
            admin.Balance += value;
            
            _repo.Update(new Guid(user.Id), user);
            _repo.Update(new Guid(admin.Id), admin);
            
            await _repo.CompleteAsync();
        }
        
        
        //=============================================================================================
        public async Task TransferMoneyFromSystemToUserAsync(ApplicationUser user, double value)
        {
            var admin = await GetAdminAsync();
            
            user.Balance += value;
            admin.Balance -= value;
            
            _repo.Update(new Guid(user.Id), user);
            _repo.Update(new Guid(admin.Id), admin);
            
            await _repo.CompleteAsync();
        }
    }
}