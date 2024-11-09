using CAPS.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CreditAndPawnShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Loan> Loans { get; set; }
        public DbSet<PawnItem> PawnItems { get; set; }
        public DbSet<PawnShop> PawnShops { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<PawnShop>().HasData(
                new PawnShop
                {
                    Id = 1,
                    Name = "GoldenVarna",
                    City = "Varna"
                },
                new PawnShop
                {
                    Id = 2,
                    Name = "MainaShop",
                    City = "Plovdiv"
                },
                new PawnShop
                {
                    Id = 3,
                    Name = "Viliger",
                    City = "Sofia"
                }
            );

           
        }

    }
}
