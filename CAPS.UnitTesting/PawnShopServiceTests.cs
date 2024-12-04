

using CAPS.Data.Data;
using CAPS.Data;
using CAPS.DataModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using CAPS.Global;

namespace CAPS.UnitTesting
{
    public class PawnShopServiceTests
    {
        private readonly PawnShopService _pawnShopService;
        private readonly ApplicationDbContext _context;

        public PawnShopServiceTests()
        {

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);


            _pawnShopService = new PawnShopService(_context);
        }

        private List<PawnShop> GetTestPawnShops()
        {
            return new List<PawnShop>
            {
             new PawnShop { Id = 1, Name = "Shop 1", City = "City 1", LocationUrl = "http://shop1.com", IsDeleted = false },
             new PawnShop { Id = 2, Name = "Shop 2", City = "City 2", LocationUrl = "http://shop2.com", IsDeleted = true }, // This one is deleted
             new PawnShop { Id = 3, Name = "Shop 3", City = "City 3", LocationUrl = "http://shop3.com", IsDeleted = false }
            };
        }

        private PawnShop GetTestPawnShopWithItems()
        {
            return new PawnShop
            {
                Id = 1,
                Name = "Shop 1",
                City = "City 1",
                LocationUrl = "http://shop1.com",
                IsDeleted = false,
                PawnedItems = new List<PawnItem>
                {
                 new PawnItem { Id = 1, Name = "Item 1", Value = 100, Category = ItemCategory.Other, AppUserId = "user1", Description = "...", IsDeleted = false },
                 new PawnItem { Id = 2, Name = "Item 2", Value = 200, Category = ItemCategory.Other, AppUserId = "user1", Description = "...", IsDeleted = true }, // Deleted item
                 new PawnItem { Id = 3, Name = "Item 3", Value = 300, Category = ItemCategory.Other, AppUserId = "user1", Description = "...", IsDeleted = false }
                }
            };
        }



        [Fact]
        public async Task GetAllPawnShopsAsync_ReturnsAllNonDeletedPawnShops()
        {
            // Arrange
            var pawnShops = GetTestPawnShops();


            _context.PawnShops.AddRange(pawnShops);
            await _context.SaveChangesAsync();

            // Act
            var result = await _pawnShopService.GetAllPawnShopsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Only non-deleted pawn shops should be returned
            Assert.All(result, ps => Assert.False(pawnShops.First(shop => shop.Id == ps.Id).IsDeleted)); // Ensure non-deleted pawn shops

           
           _context?.Dispose();
            
        }
        [Fact]
        public async Task GetPawnShopWithItemsAsync_ReturnsPawnShopWithItems()
        {
            // Arrange
            var pawnShop = GetTestPawnShopWithItems();

            var pawnShops = new List<PawnShop> { pawnShop };


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;


            using var context = new ApplicationDbContext(options);
            context.PawnShops.AddRange(pawnShops);
            await context.SaveChangesAsync();


            var service = new PawnShopService(context);

            // Act
            var result = await service.GetPawnShopWithItemsAsync(1); // Using pawn shop Id = 1

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Shop 1", result.Name);
            Assert.Equal(2, result.PawnedItems.Count); // Only 2 items should be returned (non-deleted)


            Assert.Contains(result.PawnedItems, item => item.ItemName == "Item 1");
            Assert.Contains(result.PawnedItems, item => item.ItemName == "Item 3");

            // item 2 should not be in the result
            Assert.DoesNotContain(result.PawnedItems, item => item.ItemName == "Item 2");

            
        }
    }
}


