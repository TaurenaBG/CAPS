using CAPS.DataModels;
using CAPS.ViewModels;

namespace CAPS.Services
{
    public interface IPawnShopService
    {
        Task<List<PawnShopsViewModel>> GetAllPawnShopsAsync();

        
        Task<PawnShopsViewModel> GetPawnShopWithItemsAsync(int pawnShopId);
    }
}
