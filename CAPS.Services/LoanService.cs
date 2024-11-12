using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
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
        public async Task<Loan> GetLoanDetailsAsync(int loanId)
        {
           
            var loan = await _context.Loans
                .Include(l => l.AppUser) 
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null)
            {
                throw new ArgumentException("Loan not found.");
            }

            return loan;
        }

        public async Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm)
        {
           
            var user = await _userManager.FindByIdAsync(userId);
            

            
            var loan = new Loan
            {
                AppUserId = user.Id,
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
       
    }
}
