using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> MyItems()
        {
           
            var user = await _userManager.GetUserAsync(User);

           

           
            var items = await _itemService.GetUserPawnedAndDeclinedItemsAsync(user.Id);

            
            return View(items);
        }
        public async Task<IActionResult> Redeem(int id)
        {
            var item = await _itemService.FindItemByIdAsync(id); 
           

            var user = await _userManager.GetUserAsync(User);
            

           
            var result = await _itemService.RedeemItemAsync(user, item);

            if (result)
            {
                return RedirectToAction("MyItems");
            }

           
            return RedirectToAction("InsufficientFunds");
        }

       
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemService.FindItemByIdAsync(id); 
           

            var user = await _userManager.GetUserAsync(User);
           

            
            await _itemService.DeleteItemAsync(item);

            return RedirectToAction("MyItems");
        }
        public async Task<IActionResult> InsufficientFunds()
        {
            return View();
        }

        public async Task<IActionResult> Buy()
        {
            var currentDate = DateTime.Now;
            
            var updatedItems = await _itemService.GetItemsByDueDateWithTaxAsync(currentDate);

            return View(updatedItems);
        }


        public async Task<IActionResult> BuyItem(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User); 
            var success = await _itemService.BuyItemAsync(id, currentUser);

            if (success)
            {
                
                return RedirectToAction("Index", "Home"); 
            }
            else
            {

                return RedirectToAction("InsufficientFunds", "Items");
            }
        }
        public async Task<IActionResult> BroughtItems()
        {
            
            var currentUser = await _userManager.GetUserAsync(User);

           
            var boughtItems = await _itemService.GetBoughtItemsAsync(currentUser.Id);

            return View(boughtItems); 
        }
    }
}
