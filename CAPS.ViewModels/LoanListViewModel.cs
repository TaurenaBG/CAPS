using CAPS.DataModels;


namespace CAPS.ViewModels
{
    public class LoanListViewModel
    {
        public List<Loan> ApprovedLoans { get; set; } = new List<Loan>();
        public List<Loan> DeclinedLoans { get; set; } = new List<Loan>();
    }
}
