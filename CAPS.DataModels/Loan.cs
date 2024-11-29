using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CAPS.Global;
using static CAPS.Global.GlobalConstants;

namespace CAPS.DataModels
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        [Range(LoanTermMinValue, LoanTermMaxValue, ErrorMessage = LoanTermNotInRangeErrorMsg)]
        public int LoanTerm { get; set; }

        public LoanStatus LoanStatus { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime IssuedDate { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime DueDate { get; set; }

        public bool IsDeleted { get; set; }

        

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
