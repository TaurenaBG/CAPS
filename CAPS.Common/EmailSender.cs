using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CAPS.Common
{
    public class EmailSender : IEmailSender
    {
       

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Simulate email sending 
            Console.WriteLine($"Sending email to: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {htmlMessage}");

            // Return a completed task 
            return Task.CompletedTask;
        }
    }
}
