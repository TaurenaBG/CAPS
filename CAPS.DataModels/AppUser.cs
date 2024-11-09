using Microsoft.AspNetCore.Identity;

namespace CAPS.DataModels
{
    public class AppUser : IdentityUser 
    {
        public AppUser()
        {
        }

        public AppUser(string userName) : base(userName)
        {
        }

        public string? FullName { get; set; }

        public string? ReturnUrl { get; set; }

        public List<Loan> Loans { get; set; } = new List<Loan>();
        public List<PawnItem> PawnedItems { get; set; } = new List<PawnItem>();
    }
}
