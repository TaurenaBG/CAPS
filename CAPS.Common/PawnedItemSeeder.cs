using CAPS.Data.Data;
using CAPS.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Common
{
    public class PawnedItemSeeder
    {
        public static async Task SeedPawnedItemsAsync(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            // Ensure there are PawnShops and a user to assign the items to
            var pawnShops = await context.PawnShops.ToListAsync();
            var adminUser = await userManager.FindByEmailAsync("admin@admin.com");

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
                            PawnShopId = 1, // First pawn shop
                            AppUserId = adminUser.Id
                        },
                        new PawnItem
                        {
                            Name = "Laptop",
                            Description = "MacBook Pro",
                            Value = 1500,
                            PawnShopId = 2, // Second pawn shop
                            AppUserId = adminUser.Id
                        },
                        new PawnItem
                        {
                            Name = "Watch",
                            Description = "Rolex",
                            Value = 1200,
                            PawnShopId = 3, // Third pawn shop
                            AppUserId = adminUser.Id
                        },
                        new PawnItem
                        {
                            Name = "Car",
                            Description = "Bmw 530",
                            Value = 30000,
                            PawnShopId = 1, // First pawn shop
                            AppUserId = adminUser.Id
                        }
                    };

                    // Add the items to the context
                    context.PawnItems.AddRange(pawnedItems);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

