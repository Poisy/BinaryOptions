using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Repos;

namespace Infrastructure.Services
{
    public class CurrencyService
    {
        //=============================================================================================
        private readonly IGenericRepository<CurrencyPair> _currencyRepo;

        
        //=============================================================================================
        public CurrencyService(IGenericRepository<CurrencyPair> currencyRepo)
        {
            _currencyRepo = currencyRepo;
        }
        
        
        //=============================================================================================
        public async Task<bool> Exists(string currencyName)
        {
            var currencyPair = await _currencyRepo
                .FirstOrDefaultAsync(pair => pair.Name.Equals(currencyName));

            return currencyPair != null;
        }

        //=============================================================================================
        public async Task AddAsync(string currencyName)
        {
            var notExist = !(await Exists(currencyName));
            
            if (notExist)
            {
                await _currencyRepo.AddAsync(new CurrencyPair
                {
                    Name = currencyName
                });
                await _currencyRepo.CompleteAsync();
            }
        }

        //=============================================================================================
        public async Task<CurrencyPair> GetAsync(string currencyName)
        {
            return await _currencyRepo
                .FirstOrDefaultAsync(pair => pair.Name.Equals(currencyName));
        }

        public async Task<List<CurrencyPair>> GetAllAsync()
            => (await _currencyRepo.GetAllAsync()).ToList();
    }
}