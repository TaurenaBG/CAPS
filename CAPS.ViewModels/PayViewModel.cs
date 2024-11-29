using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;
namespace CAPS.ViewModels
{
    public class PayViewModel
    {
        public int LoanId { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
        public decimal Amount { get; set; }

        public decimal LoanInitialAmount { get; set; }
        public string AppUserId { get; set; }
    }
}
