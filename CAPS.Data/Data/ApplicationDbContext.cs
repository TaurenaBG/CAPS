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

           
            

            modelBuilder.Entity<AppUser>()
           .HasMany(u => u.BroughtItems) 
           .WithOne(i => i.AppUser) 
           .HasForeignKey(i => i.AppUserId)
           .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
