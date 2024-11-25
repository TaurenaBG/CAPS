

using CAPS.Data.Data;
using CAPS.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CAPS.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                .Include(p => p.AppUser)
                .Include(p => p.PawnItem) 
                .Where(p => !p.IsDeleted) 
                .ToListAsync();
        }
        public async Task ClearAllPaymentsAsync()
        {
           
            var paymentsToDelete = _context.Payments.Where(p => !p.IsDeleted).ToList();

            if (paymentsToDelete.Any())
            {
                foreach (var payment in paymentsToDelete) 
                {
                    payment.IsDeleted = true;
                }
                await _context.SaveChangesAsync(); 
            }
        }

    }
}
