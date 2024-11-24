
using CAPS.DataModels;
using CAPS.Global;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CAPS.Services
{
    public interface IItemService
    {
        Task<PawnItem> CreateItemAsync(ItemViewModel model, string userId);
        Task<PawnItem> FindItemByIdAsync(int itemId);
        Task<List<SelectListItem>> GetPawnShopsAsync();

        Task<List<PawnItem>> GetPendingItemsAsync();
        Task UpdateItemStatusAsync(int itemId, PawnStatus status);

         Task<bool> ApproveItemAsync(int itemId, AppUser adminUser);

        Task<List<PawnItem>> GetUserPawnedAndDeclinedItemsAsync(string userId);

        Task<bool> RedeemItemAsync(AppUser user, PawnItem item);
        Task DeleteItemAsync(PawnItem item);


    }
}
