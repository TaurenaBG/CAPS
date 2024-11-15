using CAPS.DataModels;
using CAPS.Services;
using CreditAndPawnShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CreditAndPawnShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
       

        public HomeController(ILogger<HomeController> logger,
            UserManager<AppUser> userManager
           )
        {
            _logger = logger;
            _userManager = userManager;
            
        }

        
        

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                
                ViewBag.FullName = user.FullName ?? user.Email; 
                ViewBag.CurrencyAmount = user.CurrencyAmount;
            }

            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
