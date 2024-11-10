using CAPS.Data.Data;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditAndPawnShop.Controllers
{
    public class PawnShopsController : Controller
    {
        private readonly IPawnShopService _pawnShopService;

        
        public PawnShopsController(IPawnShopService pawnShopService)
        {
            _pawnShopService = pawnShopService;
        }

        
        public async Task<IActionResult> Index()
        {
            
            var pawnShops = await _pawnShopService.GetAllPawnShopsAsync();

            
            return View(pawnShops);
        }

       
        public async Task<IActionResult> PawnedItems(int pawnShopId)
        {
           
            var pawnShop = await _pawnShopService.GetPawnShopWithItemsAsync(pawnShopId);

            if (pawnShop == null)
            {
                return NotFound();  
            }

            
            return View(pawnShop);
        }
        public async Task<IActionResult> Details(int pawnShopId)
        {
            var pawnShopViewModel = await _pawnShopService.GetPawnShopWithItemsAsync(pawnShopId);
            if (pawnShopViewModel == null)
            {
                return NotFound();
            }
            return View(pawnShopViewModel);  
        }

        
        public IActionResult OpenMap(string locationUrl)
        {
            if (string.IsNullOrEmpty(locationUrl))
            {
                return BadRequest("Location URL is not provided.");
            }
            return Redirect(locationUrl);  
        }
    }
}
