using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Common
{
    public class Seeder
    {
        public static async Task SeedDBAsync(ApplicationDbContext context, UserManager<AppUser> userManager)
        {


            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");
            if (!context.PawnShops.Any())
            {
                var seedPawnShops = new List<PawnShop>
               {
                   new PawnShop
                   {
                      
                       Name = "GoldenVarna",
                       City = "Varna",
                       LocationUrl = "https://www.google.com/maps?q=43.2141,27.9147"
                   },
                   new PawnShop
                   {
                      
                       Name = "MainaShop",
                       City = "Plovdiv",
                       LocationUrl = "https://www.google.com/maps?q=42.1354,24.7455"
                   },
                   new PawnShop
                   {
                      
                       Name = "Vip",
                       City = "Sofia",
                       LocationUrl = "https://www.google.com/maps?q=42.6977,23.3219"
                   }
               };

                
                context.PawnShops.AddRange(seedPawnShops);
                await context.SaveChangesAsync(); 
            }

            var pawnShops = await context.PawnShops.ToListAsync();


            if (adminUser != null && pawnShops.Any())
            {

                if (!context.PawnItems.Any())
                {
                    var pawnedItems = new List<PawnItem>
                    {
                        new PawnItem
                        {
                            Name = "Gold Necklace",
                            Description = "24 carats gold",
                            Value = 500,
                            PawnShopId = pawnShops[0].Id, // First pawn shop
                            AppUserId = adminUser.Id,
                            Category = ItemCategory.Jewelry,
                            PawnDate = DateTime.Now.AddMonths(-2),
                             DueDate = DateTime.Now.AddMonths(-1)
                        },
                        new PawnItem
                        {
                            Name = "Laptop",
                            Description = "MacBook Pro",
                            Value = 1500,
                            PawnShopId = pawnShops[1].Id, // Second pawn shop
                            AppUserId = adminUser.Id,
                            Category = ItemCategory.Electronics,
                            PawnDate = DateTime.Now.AddMonths(-2),
                             DueDate = DateTime.Now.AddMonths(-1)
                        },
                        new PawnItem
                        {
                            Name = "Watch",
                            Description = "Rolex",
                            Value = 1200,
                            PawnShopId = pawnShops[2].Id, // Third pawn shop
                            AppUserId = adminUser.Id,
                            Category = ItemCategory.Jewelry,
                            PawnDate = DateTime.Now.AddMonths(-2),
                             DueDate = DateTime.Now.AddMonths(-1)
                        },
                        new PawnItem
                        {
                            Name = "Car",
                            Description = "Bmw 530",
                            Value = 3000,
                            PawnShopId = pawnShops[0].Id, // First pawn shop
                            AppUserId = adminUser.Id,
                            Category = ItemCategory.Vehicles,
                            PawnDate = DateTime.Now.AddMonths(-2),
                             DueDate = DateTime.Now.AddMonths(-1)
                        }
                    };


                    context.PawnItems.AddRange(pawnedItems);
                    await context.SaveChangesAsync();

                    context.ChangeTracker.Clear();

                    
                }
            }
        }
    }
}

