using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult InsufficientFunds()
        {
            return View();  
        }
    }
}
