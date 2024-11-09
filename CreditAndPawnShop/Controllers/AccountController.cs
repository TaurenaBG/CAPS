using Microsoft.AspNetCore.Mvc;

namespace CreditAndPawnShop.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
