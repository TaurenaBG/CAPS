using CAPS.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Data.Data
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

           
            //modelBuilder.Entity<PawnShop>().HasData(
            //    new PawnShop
            //    {
            //        Id = 1,
            //        Name = "GoldenVarna",
            //        City = "Varna",
            //        LocationUrl = "https://www.google.com/maps?q=43.2141,27.9147"
            //    },
            //    new PawnShop
            //    {
            //        Id = 2,
            //        Name = "MainaShop",
            //        City = "Plovdiv",
            //        LocationUrl = "https://www.google.com/maps?q=42.1354,24.7455"
            //    },
            //    new PawnShop
            //    {
            //        Id = 3,
            //        Name = "Viliger",
            //        City = "Sofia",
            //        LocationUrl = "https://www.google.com/maps?q=42.6977,23.3219"
            //    }
            //);

            modelBuilder.Entity<AppUser>()
           .HasMany(u => u.BroughtItems) 
           .WithOne(i => i.AppUser) 
           .HasForeignKey(i => i.AppUserId)
           .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
