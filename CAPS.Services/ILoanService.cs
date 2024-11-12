

using CAPS.DataModels;

namespace CAPS.Services
{
    public interface ILoanService
    {
        Task<Loan> GetLoanDetailsAsync(int loanId);
        Task<Loan> CreateLoanAsync(string userId, decimal loanAmount, int loanTerm);

       
    }
}
