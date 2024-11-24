

using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Services
{
    public class ItemService : IItemService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ItemService(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

       
        public async Task<PawnItem> CreateItemAsync(ItemViewModel model, string userId)
        {
            var pawnItem = new PawnItem
            {
                Name = model.Name,
                Description = model.Description,
                Value = model.Value,
                Category = model.Category,
                PawnDate = model.PawnDate,
                DueDate = model.DueDate,
                AppUserId = userId,
                PawnShopId = model.PawnShopId, 
                Status = PawnStatus.Pending 
            };

           
            _context.PawnItems.Add(pawnItem);
            await _context.SaveChangesAsync();

            return pawnItem;
        }

        
        public async Task<List<SelectListItem>> GetPawnShopsAsync()
        {
            var pawnShops = await _context.PawnShops
                .Select(ps => new SelectListItem
                {
                    Value = ps.Id.ToString(),
                    Text = ps.Name
                })
                .ToListAsync();

            return pawnShops;
        }

        public async Task<PawnItem> FindItemByIdAsync(int itemId)
        {
            return await _context.PawnItems.FindAsync(itemId);
        }

        
    }
}
