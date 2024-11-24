using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    public class ItemsController : Controller
    {


        private readonly IItemService _itemService;
        private readonly UserManager<AppUser> _userManager;

        public ItemsController(IItemService itemService, UserManager<AppUser> userManager)
        {
            _itemService = itemService;
            _userManager = userManager;
        }


        public async Task<IActionResult> PawnItem()
        {

            var pawnShops = await _itemService.GetPawnShopsAsync();


            var currentUserId = _userManager.GetUserId(User);


            var model = new ItemViewModel
            {
                PawnShops = pawnShops
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PawnItem(ItemViewModel model)
        {
            if (ModelState.IsValid)
            {

                var currentUserId = _userManager.GetUserId(User);


                var item = await _itemService.CreateItemAsync(model, currentUserId);


                return RedirectToAction("Details", new { id = item.Id });
            }


            return View(model);
        }


        public async Task<IActionResult> Details(int id)
        {
            var itemDetails = await _itemService.FindItemByIdAsync(id);
           
            return View(itemDetails);
        }
    }
}
