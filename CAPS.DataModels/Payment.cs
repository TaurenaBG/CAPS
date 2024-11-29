using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAPS.DataModels;
using static CAPS.Global.GlobalConstants;
public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = IsRequiredMsg)]
    [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = IsRequiredMsg)]
    public DateTime PaymentDate { get; set; }

    public bool IsDeleted { get; set; }


    public int? LoanId { get; set; } // Nullable
    [ForeignKey(nameof(LoanId))]
    public Loan Loan { get; set; }

    public int? PawnItemId { get; set; } // Nullable
    [ForeignKey(nameof(PawnItemId))]
    public PawnItem PawnItem { get; set; }

    public string AppUserId { get; set; }
    [ForeignKey(nameof(AppUserId))]
    public AppUser AppUser { get; set; }
}
