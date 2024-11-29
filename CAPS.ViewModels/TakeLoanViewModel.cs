using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;

namespace CAPS.ViewModels
{
    public class TakeLoanViewModel
    {
        [Required]
        [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
        public decimal Amount { get; set; }

        [Required]
        [Range(LoanTermMinValue, LoanTermMaxValue, ErrorMessage = LoanTermNotInRangeErrorMsg)]
        public int LoanTerm { get; set; }
    }
}
