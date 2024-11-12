using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    public class ItemsController : Controller
    {
        
        public IActionResult Buy()
        {
            
            return View();
        }

        
        public IActionResult Sell()
        {
           
            return View();
        }
    }
}
