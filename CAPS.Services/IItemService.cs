
using CAPS.DataModels;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CAPS.Services
{
    public interface IItemService
    {
        Task<PawnItem> CreateItemAsync(ItemViewModel model, string userId);
        Task<PawnItem> FindItemByIdAsync(int itemId);
        Task<List<SelectListItem>> GetPawnShopsAsync();


    }
}
