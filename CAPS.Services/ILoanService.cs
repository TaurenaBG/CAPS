

using CAPS.DataModels;
using CAPS.ViewModels;

namespace CAPS.Services
{
    public interface ILoanService
    {
        Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm);
        Task<Loan> GetLoanDetailsAsync(int loanId);
        Task<Loan> GetLoanByIdAsync(int loanId);

    }
}
