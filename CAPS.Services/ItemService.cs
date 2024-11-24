

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
        private readonly IPawnShopAdminService _pawnShopAdminService;

        public ItemService(UserManager<AppUser> userManager,
            IPawnShopAdminService pawnShopAdminService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            _pawnShopAdminService = pawnShopAdminService;
        }

       
        public async Task<PawnItem> CreateItemAsync(ItemViewModel model, string userId)
        {
            var pawnItem = new PawnItem
            {
                Name = model.Name,
                Description = model.Description,
                Value = model.Value,
                Category = model.Category,
                PawnDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(1),
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

        public async Task UpdateItemStatusAsync(int itemId, PawnStatus status)
        {
            var item = await _context.PawnItems
                                      .FirstOrDefaultAsync(i => i.Id == itemId && !i.IsDeleted);

            

            
            item.Status = status;

            
            await _context.SaveChangesAsync();
        }
        public async Task<List<PawnItem>> GetPendingItemsAsync()
        {
            return await _context.PawnItems
                                 .Where(item => item.Status == PawnStatus.Pending && !item.IsDeleted)
                                 .ToListAsync();
        }
        public async Task<bool> ApproveItemAsync(int itemId, AppUser user)
        {
           
            var item = await FindItemByIdAsync(itemId);




            var adminUser = user;

            
            var pawnUser = await _userManager.FindByIdAsync(item.AppUserId);

           

            
            if (adminUser.CurrencyAmount < item.Value)
            {
                return false; 
            }

            
            adminUser.CurrencyAmount -= item.Value;

            
            pawnUser.CurrencyAmount += item.Value;

            
            var adminResult = await _userManager.UpdateAsync(adminUser);
            var pawnUserResult = await _userManager.UpdateAsync(pawnUser);

            
            

            
            item.Status = PawnStatus.Pawned;
            _context.SaveChangesAsync();

            return true; 
        }
        public async Task<List<PawnItem>> GetUserPawnedAndDeclinedItemsAsync(string userId)
        {
           
            return await _context.PawnItems
                .Where(item => item.AppUserId == userId && (item.Status == PawnStatus.Pawned || item.Status == PawnStatus.Declined) && !item.IsDeleted)
                .ToListAsync();
        }
        public async Task<bool> RedeemItemAsync(AppUser user, PawnItem item)
        {
            var totalAmount = (0.2m* item.Value) + item.Value ;

            if (user.CurrencyAmount < totalAmount)
            {
                return false; 
            }

            
            item.Status = PawnStatus.Redeemed; 
            _context.Update(item);

            
            user.CurrencyAmount -= totalAmount;

            var owner = await _pawnShopAdminService.GetAdminUserAsync();
            
            owner.CurrencyAmount += totalAmount;

                _context.Update(user);
            

            await _context.SaveChangesAsync();

            return true;
        }

        
        public async Task DeleteItemAsync(PawnItem item)
        {
            item.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<PawnItemViewModel>> GetItemsByDueDateWithTaxAsync(DateTime currentDate)
        {
            var items = await _context.PawnItems
                .Where(item => item.DueDate < currentDate && !item.IsDeleted && item.Status == PawnStatus.Pawned)
                .ToListAsync();

            
            var updatedItems = items.Select(item => new PawnItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                OriginalValue = item.Value,
                ValueWithTax = item.Value * 1.2m,  // Add 20%
                Status = item.Status
            }).ToList();

            return updatedItems;
        }
        public async Task<bool> BuyItemAsync(int itemId, AppUser currentUser)
        {
            
            var item = await _context.PawnItems
                .Where(i => i.Id == itemId && !i.IsDeleted && i.Status == PawnStatus.Pawned)
                .FirstOrDefaultAsync();

           
            
            if (currentUser.CurrencyAmount < item.Value * 1.2m) 
            {
                return false; // Not enough funds to buy the item
            }

            
            currentUser.CurrencyAmount -= item.Value * 1.2m;

            var owner = await _pawnShopAdminService.GetAdminUserAsync();
            owner.CurrencyAmount += item.Value * 1.2m;

           
            item.Status = PawnStatus.Sold;
            item.IsDeleted = true;

            currentUser.BroughtItems.Add(item);

            _context.PawnItems.Update(item);
            _context.Users.Update(currentUser);
            _context.Users.Update(owner);

            
             await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<PawnItem>> GetBoughtItemsAsync(string userId)
        {
            return await _context.PawnItems
                .Where(item => item.AppUserId == userId && item.Status == PawnStatus.Sold)
                .ToListAsync();
        }
    }
}
