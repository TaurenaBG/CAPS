using CAPS.DataModels;
using CAPS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Services
{
    public interface IPawnShopAdminService
    {
        Task CreatePawnShopAsync(PawnShopsViewModel model);
        Task UpdatePawnShopAsync(PawnShopsViewModel model);
        Task DeletePawnShopAsync(int id);
        Task<PawnShopsViewModel> GetPawnShopByIdAsync(int id);
        Task<AppUser> GetAdminUserAsync();
    }
}
