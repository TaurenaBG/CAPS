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
        public decimal Amount { get; set; }

        public LoanStatus LoanStatus { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime IssuedDate { get; set; }

        [Required(ErrorMessage = IsRequiredMsg)]
        public DateTime DueDate { get; set; }

        public bool IsDeleted { get; set; }
        public int PawnShopId { get; set; }

        [ForeignKey(nameof(PawnShopId))]
        public PawnShop PawnShop { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
