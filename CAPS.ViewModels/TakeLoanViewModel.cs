using CAPS.Global;
using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;

namespace CAPS.ViewModels
{
    public class TakeLoanViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public decimal Amount { get; set; }

        public int LoanTerm { get; set; }

        public LoanStatus LoanStatus { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime IssuedDate { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime DueDate { get; set; }
    }
}
