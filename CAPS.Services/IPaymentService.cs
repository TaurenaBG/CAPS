

using CAPS.DataModels;

namespace CAPS.Services
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllPaymentsAsync();
        Task ClearAllPaymentsAsync();
    }
}
