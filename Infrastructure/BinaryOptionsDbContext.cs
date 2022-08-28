using System;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class BinaryOptionsDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        //=============================================================================================
        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Option> Options { get; set; }

        public DbSet<CurrencyPair> CurrencyPairs { get; set; }

        public DbSet<BidResult> BidResults { get; set; }

        public DbSet<Withdraw> Withdraws { get; set; }
        
        
        //=============================================================================================
        public BinaryOptionsDbContext(DbContextOptions<BinaryOptionsDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        
        
        
        //=============================================================================================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
            {
                Id = new Guid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                PasswordHash = new PasswordHasher<ApplicationUser>()
                    .HashPassword(null, _configuration["Admin:Password"]),
                Email = _configuration["Admin:Email"],
                NormalizedEmail = _configuration["Admin:Email"],
                Nationality = "BG",
                Balance = 100000
            });
        }
    }
}