using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CreditAndPawnShop.Controllers
{
    [Authorize]
    public class LoansController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly UserManager<AppUser> _userManager;

        public LoansController(ILoanService loanService, UserManager<AppUser> userManager)
        {
            _loanService = loanService;
            _userManager = userManager;
        }

        
        public IActionResult TakeLoan()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(new TakeLoanViewModel());
        }


        [HttpPost]
        
        public async Task<IActionResult> TakeLoan(TakeLoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
               

                
                var loan = await _loanService.CreateLoanAsync(user.Id, model.Amount, model.LoanTerm);

                
                return RedirectToAction("LoanDetails", new { id = loan.Id });
            }

           
            return View(model);
        }

        
        public async Task<IActionResult> LoanDetails(int id)
        {
            try
            {
                var loan = await _loanService.GetLoanDetailsAsync(id);
                if (loan == null)
                {
                    return NotFound();
                }

                return View(loan);
            }
            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }
        }
    }



}

