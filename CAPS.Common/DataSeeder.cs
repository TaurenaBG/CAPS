using CAPS.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CAPS.Common
{
    public class DataSeeder
    {
        public static async Task SeedAdminRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole("Admin");
            await roleManager.CreateAsync(adminRole);
        }

        public static async Task SeedUserRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var userRole = new IdentityRole("User");
            await roleManager.CreateAsync(userRole);
        }

        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminUser = new AppUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
               
            };

            var result = await userManager.CreateAsync(adminUser, "admin123");

            if (result.Succeeded)
            {
                // Assign the Admin role to the user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
