using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CAPS.UnitTesting
{
    public class PaymentServiceTests
    {
        private List<Payment> GetTestPayments()
        {
            return new List<Payment>()
            {
                new Payment
                {
                    Id = 1,
                    Amount = 100,
                    PaymentDate = DateTime.Now,
                    IsDeleted = false,
                    AppUserId = "user1",
                    PawnItemId = 1,
                    AppUser = new AppUser { Id = "user1"},
                    PawnItem = new PawnItem { Id = 1, Name = "Pawn Item 1", Value = 100, AppUserId = "user1", Description = "...", Category = ItemCategory.Other }
                },
                new Payment
                {
                    Id = 2,
                    Amount = 200,
                    PaymentDate = DateTime.Now.AddDays(-1),
                    IsDeleted = true, // This one is deleted
                    AppUserId = "user2",
                    PawnItemId = 2,
                    AppUser = new AppUser { Id = "user2"},
                    PawnItem = new PawnItem { Id = 2, Name = "Pawn Item 2", Value = 200, AppUserId = "user2", Description = "...", Category = ItemCategory.Other }
                }
            };
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ReturnsNonDeletedPayments()
        {
            // Arrange
            var payments = GetTestPayments();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();

           
            var service = new PaymentService(context);

            // Act
            var result = await service.GetAllPaymentsAsync();

            // Assert
            Assert.Equal(1, result.Count); // Only 1 payment should be returned (non-deleted)
            Assert.DoesNotContain(result, p => p.Id == 2);
            Assert.Contains(result, p => p.Id == 1);

            
        }

        [Fact]
        public async Task ClearAllPaymentsAsync_MarksAllPaymentsAsDeleted()
        {
            // Arrange
            var payments = GetTestPayments();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();

           
            var service = new PaymentService(context);

            // Act
            await service.ClearAllPaymentsAsync();

            // Assert
            var result = await context.Payments.ToListAsync();
            Assert.All(result, payment => Assert.True(payment.IsDeleted)); // All payments should be marked as deleted

           
        }
    }
}

