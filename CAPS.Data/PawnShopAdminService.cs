using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Data
{
    public class PawnShopAdminService : IPawnShopAdminService
    {
        private readonly ApplicationDbContext _context;

        public PawnShopAdminService(ApplicationDbContext context)
        {
            _context = context;
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
                    LocationUrl = ps.LocationUrl
                })
                .FirstOrDefaultAsync();

            return pawnShop;
        }
    }
}
