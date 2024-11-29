using CAPS.Global;



namespace CAPS.ViewModels
{
    public class LoanDetailsViewModel
    {
        public int LoanId { get; set; }

        public decimal Amount { get; set; }
        public int LoanTerm { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime DueDate { get; set; }
        public LoanStatus LoanStatus { get; set; } 
    }
}
