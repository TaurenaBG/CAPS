using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAPS.Global;

namespace CAPS.DataModels
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public LoanStatus LoanStatus { get; set; }

        [Required]
        public DateTime IssuedDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

       
        public int PawnShopId { get; set; }

        [ForeignKey(nameof(PawnShopId))]
        public PawnShop PawnShop { get; set; }

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
