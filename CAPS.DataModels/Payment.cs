using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.DataModels
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }



        
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
}
