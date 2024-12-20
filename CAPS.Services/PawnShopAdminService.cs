﻿using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace CAPS.Data
{
    public class PawnShopAdminService : IPawnShopAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public PawnShopAdminService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task CreatePawnShopAsync(PawnShopsViewModel model)
        {
            var pawnShop = new PawnShop
            {
                Name = model.Name,
                City = model.City,
                LocationUrl = model.LocationUrl,
                IsDeleted = false
            };

            _context.PawnShops.Add(pawnShop);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePawnShopAsync(PawnShopsViewModel model)
        {
            var pawnShop = await _context.PawnShops.FirstOrDefaultAsync(ps => ps.Id == model.Id && !ps.IsDeleted);
            if (pawnShop != null)
            {
                pawnShop.Name = model.Name;
                pawnShop.City = model.City;
                pawnShop.LocationUrl = model.LocationUrl;

                _context.PawnShops.Update(pawnShop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePawnShopAsync(int id)
        {
            var pawnShop = await _context.PawnShops.FirstOrDefaultAsync(ps => ps.Id == id && !ps.IsDeleted);
            if (pawnShop != null)
            {
                pawnShop.IsDeleted = true; // Soft delete
                _context.PawnShops.Update(pawnShop);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PawnShopsViewModel> GetPawnShopByIdAsync(int id)
        {
            var pawnShop = await _context.PawnShops
                .Where(ps => ps.Id == id)
                .Select(ps => new PawnShopsViewModel
                {
                    Id = ps.Id,
                    Name = ps.Name,
                    City = ps.City,
                    LocationUrl = ps.LocationUrl,

                })
                .FirstOrDefaultAsync();

            return pawnShop;
        }
        public async Task<AppUser> GetAdminUserAsync()
        {
            
            var adminRole = await _roleManager.FindByNameAsync("Admin");

            if (adminRole != null)
            {
                
                var usersInRole = await _userManager.GetUsersInRoleAsync(adminRole.Name);

               
                return usersInRole.Where(r => r.FullName == "Owner").FirstOrDefault();
            }

            return null; 
        }
    }
}
