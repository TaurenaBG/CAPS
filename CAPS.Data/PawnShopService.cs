using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Data
{
    public class PawnShopService : IPawnShopService
    {
        private readonly ApplicationDbContext _context;

        public PawnShopService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<PawnShopsViewModel>> GetAllPawnShopsAsync()
        {
            var pawnShops = await _context.PawnShops.ToListAsync();

            var pawnShopsViewModel = pawnShops
                .Where(ps => !ps.IsDeleted)
                .Select(ps => new PawnShopsViewModel
            {
                Id = ps.Id,
                Name = ps.Name,
                City = ps.City,
                LocationUrl = ps.LocationUrl
            }).ToList();

            return pawnShopsViewModel;
        }


        public async Task<PawnShopsViewModel> GetPawnShopWithItemsAsync(int pawnShopId)
        {
            var pawnShop = await _context.PawnShops
                .Include(ps => ps.PawnedItems)
                .Where(ps => !ps.IsDeleted)
                .FirstOrDefaultAsync(ps => ps.Id == pawnShopId);

            if (pawnShop == null)
                return null;

            
            var pawnShopViewModel = new PawnShopsViewModel
            {
                Id = pawnShop.Id,
                Name = pawnShop.Name,
                City = pawnShop.City,
                
                PawnedItems = pawnShop.PawnedItems
                .Where(ps => !ps.IsDeleted)
                .Select(pi => new PawnedItemViewModel
                {
                    Id = pi.Id,
                    ItemName = pi.Name,
                    Value = pi.Value
                }).ToList()
            };

            return pawnShopViewModel;
        }
    }
}
