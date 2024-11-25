using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditAndPawnShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IPawnShopAdminService _pawnShopAdminService;
        private readonly IPawnShopService _pawnShopService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IItemService _itemService;
        private readonly IPaymentService _paymentService;

        public AdminController(IPawnShopAdminService pawnShopAdminService,
            IPawnShopService pawnShopService,
             IPaymentService paymentService,
            IItemService itemService,
            UserManager<AppUser> userManager)
        {
            _pawnShopAdminService = pawnShopAdminService;
            _pawnShopService = pawnShopService;
            _userManager = userManager;
            _itemService = itemService;
            _paymentService = paymentService;
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

        public async Task<IActionResult> ApproveItem(int itemId)
        {
            
            var adminUser = await _userManager.GetUserAsync(User);

            if (adminUser == null)
            {
                return Unauthorized(); // If the admin user is not found
            }

           
            bool success = await _itemService.ApproveItemAsync(itemId, adminUser);

            if (!success)
            {
                
                return RedirectToAction("InsufficientFunds");
            }

            
            return RedirectToAction("ManageItems");
        }

        public IActionResult InsufficientFunds()
        {
            return View();
        }


        public async Task<IActionResult> DeclineItem(int itemId)
        {
                         
                await _itemService.UpdateItemStatusAsync(itemId, PawnStatus.Declined);
             
                return RedirectToAction(nameof(ManageItems));
    
        }

        
        public async Task<IActionResult> ManageItems()
        {
            
            var pendingItems = await _itemService.GetPendingItemsAsync();

           
            return View(pendingItems);
        }
        [HttpGet]
        public async Task<IActionResult> ManagePayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return View(payments);
        }

        [HttpPost]
        public IActionResult ClearPayments()
        {
            _paymentService.ClearAllPaymentsAsync(); 
            return RedirectToAction("ManagePayments");
        }
    }

}

