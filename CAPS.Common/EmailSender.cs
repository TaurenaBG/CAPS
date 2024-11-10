using Microsoft.AspNetCore.Identity.UI.Services;

namespace CAPS.Common
{
    public class EmailSender : IEmailSender
    {
       

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
            // Return a completed task 
            return Task.CompletedTask;
        }
    }
}
