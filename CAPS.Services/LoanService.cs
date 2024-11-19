using CAPS.Data;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPawnShopAdminService _pawnShopAdminService;

        public LoanService(ApplicationDbContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPawnShopAdminService pawnShopAdminService)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _pawnShopAdminService = pawnShopAdminService;
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


        public async Task<Loan> GetLoanByIdAsync(int loanId)
        {
            var loan = await _context.Loans
                .Where(l => l.Id == loanId && !l.IsDeleted)
                .FirstOrDefaultAsync();


            
            //var loanDetailsViewModel = new LoanDetailsViewModel
            //{
            //    LoanId = loan.Id,
            //    Amount = loan.Amount,
            //    LoanTerm = loan.LoanTerm,
            //    IssuedDate = loan.IssuedDate,
            //    DueDate = loan.DueDate,
            //    LoanStatus = loan.LoanStatus
            //};

            return loan;
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
        public async Task<List<Loan>> FindAllApprovedLoansAsync(string userId)
        {
            return await _context.Loans
                .Where(l => l.AppUserId == userId && l.LoanStatus == LoanStatus.Aproved && !l.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Loan>> FindAllDeclinedLoansAsync(string userId)
        {
            return await _context.Loans
                .Where(l => l.AppUserId == userId && l.LoanStatus == LoanStatus.Declined && !l.IsDeleted)
                .ToListAsync();
        }
        public async Task<bool> PayLoan(int loanId, decimal paymentAmount, string userId)
        {
            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.Id == loanId && !l.IsDeleted);
            var totalAmountDue = loan.Amount + (loan.Amount * 0.20m); // 20% tax added

            var currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser.CurrencyAmount < paymentAmount)
            {
                return false;
            }

            var payment = new Payment
            {
                LoanId = loanId,
                Amount = paymentAmount,
                PaymentDate = DateTime.Now,
                AppUserId = userId
            };

            _context.Payments.Add(payment);

            if (paymentAmount >= totalAmountDue)
            {
                var excessAmount = paymentAmount - totalAmountDue;

                currentUser.CurrencyAmount -= totalAmountDue;
                currentUser.CurrencyAmount += excessAmount;

                var owner = await _pawnShopAdminService.GetAdminUserAsync();
                if (owner != null)
                {
                    owner.CurrencyAmount += totalAmountDue;
                }

                loan.LoanStatus = LoanStatus.PaidOff;

                _context.SaveChanges();
            }
            else
            {
                var amountForLoan = paymentAmount * 0.80m; // 80% goes to the loan
                var amountForAdmin = paymentAmount * 0.20m; // 20% goes to admin's currency

                loan.Amount -= amountForLoan;

                var admin = await _pawnShopAdminService.GetAdminUserAsync();
                if (admin != null)
                {
                    admin.CurrencyAmount += amountForAdmin;
                    _context.Users.Update(admin);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }





    }
}
