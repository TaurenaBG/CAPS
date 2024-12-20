﻿using CAPS.DataModels;
using Microsoft.AspNetCore.Identity;

namespace CAPS.Common
{
    public class AdminRoleSeeder
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
                CurrencyAmount = 15000,  
                FullName = "Owner"        

            };

            var result = await userManager.CreateAsync(adminUser, "admin123"); // Admin account password

            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
           
        }
    }
}
