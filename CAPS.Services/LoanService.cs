using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace CAPS.Services
{
    public class LoanService : ILoanService
    {
        private readonly ApplicationDbContext _context;

        public LoanService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new loan
        public async Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm)
        {
            var loan = new Loan
            {
                AppUserId = userId,
                Amount = loanAmount,
                LoanTerm = loanTerm,
                IssuedDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(loanTerm),
                LoanStatus = LoanStatus.Pending // Assuming LoanStatus is an enum
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();

            return loan;
        }

        // Get loan details by loanId
        public async Task<Loan> GetLoanDetailsAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.AppUser)  // Assuming Loan has a reference to AppUser
                .FirstOrDefaultAsync(l => l.Id == loanId);

            return loan;
        }

        // Get loan by ID (might be useful for additional queries)
        public async Task<Loan> GetLoanByIdAsync(int loanId)
        {
            return await _context.Loans
                .Where(l => l.Id == loanId && !l.IsDeleted) // Assuming IsDeleted field
                .FirstOrDefaultAsync();
        }
    }
}
