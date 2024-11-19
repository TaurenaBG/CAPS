using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
            return View(new TakeLoanViewModel());
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            var loanDetailsViewModel = await _loanService.GetLoanDetailsAsync(id);

            

            return View(loanDetailsViewModel);
        }

        public async Task<IActionResult> GetLoanById(int id)
        {
            var loanDetailsViewModel = await _loanService.GetLoanByIdAsync(id);

            

            return View("LoanDetails", loanDetailsViewModel); 
        }
        public async Task<IActionResult> ManageLoans()
        {
            var pendingLoans = await _loanService.GetPendingLoansAsync();
            return View(pendingLoans);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLoan(int id)
        {
            

            string adminUserId = _userManager.GetUserId(User);

            var  succes = await _loanService.ApproveLoanAsync(id, adminUserId);

            if (!succes)
            {
                
                 return View("InsufficientFunds");
            }

            
            return RedirectToAction(nameof(ManageLoans));
        }

        [HttpPost]
        public async Task<IActionResult> DeclineLoan(int id)
        {
            bool success = await _loanService.DeclineLoanAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(ManageLoans));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            bool success = await _loanService.DeleteLoanAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(ManageLoans));
        }
       

    }



}

