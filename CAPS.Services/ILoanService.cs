

using CAPS.DataModels;
using CAPS.ViewModels;

namespace CAPS.Services
{
    public interface ILoanService
    {
        Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm);
        Task<LoanDetailsViewModel> GetLoanDetailsAsync(int loanId);
        Task<Loan> GetLoanByIdAsync(int loanId);
        Task<List<Loan>> GetPendingLoansAsync();
        Task<bool> ApproveLoanAsync(int loanId, string adminUserId);
        Task<bool> DeclineLoanAsync(int loanId);
        Task<bool> DeleteLoanAsync(int loanId);
        Task<List<Loan>> FindAllApprovedLoansAsync(string userId);
        Task<List<Loan>> FindAllDeclinedLoansAsync(string userId);
        Task<bool> PayLoan(int loanId, decimal paymentAmount, string userId);





    }
}
