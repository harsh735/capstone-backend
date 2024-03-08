using Authentication1.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RegisterUser> Users { get; set; }
        public DbSet<UserBankDetails> UserBankDetails { get; set; }

        public DbSet<GoogleUserData> GoogleUserData { get; set; }

        public DbSet<InvestmentModel> InvestmentInfo { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegisterUser>().HasKey(e => e.Email);
            base.OnModelCreating(modelBuilder);
        }
    }

}
