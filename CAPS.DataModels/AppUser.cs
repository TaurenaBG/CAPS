using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAPS.DataModels
{
    public class AppUser : IdentityUser 
    {
        public AppUser()
        {
        }

        public bool IsDeleted { get; set; }

        public string? FullName { get; set; }

        public string? ReturnUrl { get; set; }

        public decimal CurrencyAmount { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<PawnItem> PawnedItems { get; set; } = new List<PawnItem>();

        
        public List<PawnItem> BroughtItems { get; set; } = new List<PawnItem>();
    }
}
