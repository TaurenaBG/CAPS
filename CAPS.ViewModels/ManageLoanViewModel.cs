using CAPS.Global;
using System.ComponentModel.DataAnnotations.Schema;

using CAPS.DataModels;

namespace CAPS.ViewModels
{
    public class ManageLoanViewModel
    {
        public int Id { get; set; }

        
        public decimal Amount { get; set; }

        public int LoanTerm { get; set; }

        public LoanStatus LoanStatus { get; set; }

       
        public DateTime IssuedDate { get; set; }

       
        public DateTime DueDate { get; set; }

        public bool IsDeleted { get; set; }

       

        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }
    }
}
