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

        // GET: Loan/Create
        public IActionResult TakeLoan()
        {
            return View(new TakeLoanViewModel());
        }

        // POST: Loan/Create
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
            return View(model); // If there are validation errors, return the same view
        }

        // GET: Loan/LoanDetails/5
        public async Task<IActionResult> LoanDetails(int id)
        {
            var loan = await _loanService.GetLoanDetailsAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            var loanDetailsViewModel = new LoanDetailsViewModel
            {
                LoanId = loan.Id,
                Amount = loan.Amount,
                LoanTerm = loan.LoanTerm,
                IssuedDate = loan.IssuedDate,
                DueDate = loan.DueDate,
                LoanStatus = loan.LoanStatus
            };

            return View(loanDetailsViewModel);
        }

        // GET: Loan/GetLoanById/5
        public async Task<IActionResult> GetLoanById(int id)
        {
            var loan = await _loanService.GetLoanByIdAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            var loanDetailsViewModel = new LoanDetailsViewModel
            {
                LoanId = loan.Id,
                Amount = loan.Amount,
                LoanTerm = loan.LoanTerm,
                IssuedDate = loan.IssuedDate,
                DueDate = loan.DueDate,
                LoanStatus = loan.LoanStatus
            };

            return View("LoanDetails", loanDetailsViewModel); // Reuse LoanDetails view
        }

    }



}

