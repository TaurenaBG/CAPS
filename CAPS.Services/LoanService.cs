using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



namespace CAPS.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LoanService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm)
        {
            var loan = new Loan
            {
                AppUserId = userId,
                Amount = loanAmount,
                LoanTerm = loanTerm,
                IssuedDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(loanTerm),
                LoanStatus = LoanStatus.Pending 
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }


        public async Task<LoanDetailsViewModel> GetLoanDetailsAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.AppUser)  
                .FirstOrDefaultAsync(l => l.Id == loanId);

            

            
            var loanDetailsViewModel = new LoanDetailsViewModel
            {
                LoanId = loan.Id,
                Amount = loan.Amount,
                LoanTerm = loan.LoanTerm,
                IssuedDate = loan.IssuedDate,
                DueDate = loan.DueDate,
                LoanStatus = loan.LoanStatus
            };

            return loanDetailsViewModel;
        }


        public async Task<LoanDetailsViewModel> GetLoanByIdAsync(int loanId)
        {
            var loan = await _context.Loans
                .Where(l => l.Id == loanId && !l.IsDeleted)
                .FirstOrDefaultAsync();


            
            var loanDetailsViewModel = new LoanDetailsViewModel
            {
                LoanId = loan.Id,
                Amount = loan.Amount,
                LoanTerm = loan.LoanTerm,
                IssuedDate = loan.IssuedDate,
                DueDate = loan.DueDate,
                LoanStatus = loan.LoanStatus
            };

            return loanDetailsViewModel;
        }

        public async Task<List<Loan>> GetPendingLoansAsync()
        {
            return await _context.Loans
                                  .Include(l => l.AppUser)
                                 .Where(l => l.LoanStatus == LoanStatus.Pending && !l.IsDeleted)
                                 .ToListAsync();
        }

        public async Task<bool> ApproveLoanAsync(int loanId, string adminUserId)
        {
           
            var loan = await _context.Loans
                                      .Include(l => l.AppUser)
                                      .FirstOrDefaultAsync(l => l.Id == loanId);
            
            
            var owner = await _userManager.FindByIdAsync(adminUserId);
           

            
            if (owner.CurrencyAmount < loan.Amount)
            {
                return false; 
            }

            
            owner.CurrencyAmount -= loan.Amount;  
            loan.AppUser.CurrencyAmount += loan.Amount;  
            loan.LoanStatus = LoanStatus.Aproved; 

           
            _context.Users.Update(owner);
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            return true;  // Loan approved successfully
        }

        public async Task<bool> DeclineLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return false;
            }

            loan.LoanStatus = LoanStatus.Declined; 
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLoanAsync(int loanId)
        {
            var loan = await _context.Loans.FindAsync(loanId);
            if (loan == null)
            {
                return false;
            }

            loan.IsDeleted = true;
            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();
            return true;
        }
        


    }
}
