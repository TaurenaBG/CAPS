using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static CAPS.Global.GlobalConstants;

namespace CAPS.DataModels
{
    public class AppUser : IdentityUser 
    {
        public AppUser()
        {
        }

        public bool IsDeleted { get; set; }

        [MaxLength(UserFullNameMaxLenght,ErrorMessage = UserFullNameMaxLenghtErrorMsg)]
        public string? FullName { get; set; }

        public string? ReturnUrl { get; set; }

        [Range(CurrencyMinAmount, CurrencyMaxAmount, ErrorMessage = CurrencyNotInRange)]
        public decimal CurrencyAmount { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<PawnItem> PawnedItems { get; set; } = new List<PawnItem>();

        
        public List<PawnItem> BroughtItems { get; set; } = new List<PawnItem>();
    }
}
