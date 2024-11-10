using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPawnShopAdminService _pawnShopAdminService;
        private readonly IPawnShopService _pawnShopService;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(IPawnShopAdminService pawnShopAdminService, IPawnShopService pawnShopService,
            UserManager<AppUser> userManager)
        {
            _pawnShopAdminService = pawnShopAdminService;
            _pawnShopService = pawnShopService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); 
            var roles = await _userManager.GetRolesAsync(user); 

            if (roles.Contains("Admin"))
            {
               
                var pawnShops = await _pawnShopService.GetAllPawnShopsAsync();
                return View(pawnShops);
            }

            
            return Unauthorized();
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(PawnShopsViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _pawnShopAdminService.CreatePawnShopAsync(model);
                return RedirectToAction("Index", "PawnShops");
            }
            return View(model);
        }

        
        public async Task<IActionResult> Edit(int id)
        {
            var pawnShop = await _pawnShopAdminService.GetPawnShopByIdAsync(id);
            if (pawnShop == null)
            {
                return NotFound();
            }
            return View(pawnShop);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PawnShopsViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _pawnShopAdminService.UpdatePawnShopAsync(model);
                return RedirectToAction("Index", "PawnShops");
            }

            return View(model);
        }

       
        public async Task<IActionResult> Delete(int id)
        {
            var pawnShop = await _pawnShopAdminService.GetPawnShopByIdAsync(id);
            if (pawnShop == null)
            {
                return NotFound();
            }
            return View(pawnShop);
        }

       
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _pawnShopAdminService.DeletePawnShopAsync(id);
            return RedirectToAction("Index", "PawnShops");
        }
    }

}

