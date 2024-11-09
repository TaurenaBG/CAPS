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
    }
}
