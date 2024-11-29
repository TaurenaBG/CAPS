using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageLoans()
        {
            var pendingLoans = await _loanService.GetPendingLoansAsync();
            return View(pendingLoans);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeclineLoan(int id)
        {
            bool success = await _loanService.DeclineLoanAsync(id);
            if (!success)
            {
                return NotFound();
            }
                   
             return RedirectToAction("ManageLoans");

            
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            bool success = await _loanService.DeleteLoanAsync(id);
            if (!success)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
            {
               
                return RedirectToAction("ManageLoans");
            }
            else
            {
                
                return RedirectToAction("PayLoan");
            }
        }
        public async Task<IActionResult> PayLoan()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;



            var approvedLoans = await _loanService.FindAllApprovedLoansAsync(userId);
            var declinedLoans = await _loanService.FindAllDeclinedLoansAsync(userId);

            
            var viewModel = new LoanListViewModel
            {
                ApprovedLoans = approvedLoans,
                DeclinedLoans = declinedLoans
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDeclinedLoan(int id)
        {
            
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser?.Id;

            
            var success = await _loanService.DeleteLoanAsync(id);

            
            return RedirectToAction("PayLoan");
        }

        [HttpGet]
        public async Task<IActionResult> PayLoanForm(int loanId)
        {
            var loan = await _loanService.GetLoanByIdAsync(loanId);
            var currentUser = await _userManager.GetUserAsync(User);



            var totalAmountDue = loan.Amount + (loan.Amount * 0.20m); // 20% tax added

            var viewModel = new PayViewModel
            {
                LoanId = loanId,
                Amount = totalAmountDue,
                AppUserId = currentUser.Id,
                
            };

            return View(viewModel);
        }
        [HttpPost]
        
        public async Task<IActionResult> PayLoanForm(PayViewModel model)
        {
           

            var currentUser = await _userManager.GetUserAsync(User);
            var success = await _loanService.PayLoanAsync(model, model.Amount, model.AppUserId);

            if (success)
            {
                // If payment succeeds, redirect to confirmation page
                return RedirectToAction("PayLoanConfirmation", new { loanId = model.LoanId });
            }

            return View(model);  
        }


        public async Task<IActionResult> PayLoanConfirmation(int loanId)
        {
            
            var loan = await _loanService.GetLoanByIdAsync(loanId);
 
            return View(loan);
        }
    }


}




