using System.ComponentModel.DataAnnotations;

namespace CAPS.ViewModels
{
    public class TakeLoanViewModel
    {
        [Required]
        [Range(1, 1000000, ErrorMessage = "Amount must be between 1 and 1,000,000")]
        public decimal Amount { get; set; }

        [Required]
        [Range(1, 60, ErrorMessage = "Loan term must be between 1 and 60 months")]
        public int LoanTerm { get; set; }
    }
}
