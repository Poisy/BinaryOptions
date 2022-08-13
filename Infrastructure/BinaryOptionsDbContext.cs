using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BinaryOptionsDbContext : IdentityDbContext<ApplicationUser>
    {
        //=============================================================================================
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<CurrencyPair> CurrencyPairs { get; set; }

        public DbSet<BidResult> BidResults { get; set; }

        public DbSet<Withdraw> Withdraws { get; set; }
        
        
        //=============================================================================================
        public BinaryOptionsDbContext(DbContextOptions<BinaryOptionsDbContext> options) : base(options)
        {
            
        }
        
        
        
        //=============================================================================================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}