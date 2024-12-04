using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CAPS.UnitTesting;
public class ItemServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IPawnShopAdminService> _pawnShopAdminServiceMock;
    private readonly ApplicationDbContext _context;
    private readonly ItemService _itemService;

    // Shared fields
    private readonly string _userId = "user123";
    private readonly string _adminUserId = "admin123";
    private readonly int _itemValue = 1000;
    private readonly int _itemId = 1;

    public ItemServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        _userManagerMock = new Mock<UserManager<AppUser>>(
            new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);

        _pawnShopAdminServiceMock = new Mock<IPawnShopAdminService>();

        _itemService = new ItemService(_userManagerMock.Object, _pawnShopAdminServiceMock.Object, _context);
    }

    // Helper method to create a user
    private async Task<AppUser> CreateUserAsync(string userId, decimal currencyAmount)
    {
        var user = new AppUser { Id = userId, CurrencyAmount = currencyAmount };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    // Helper method to create an item
    private async Task<PawnItem> CreateItemAsync(string userId, decimal value)
    {
        var item = new PawnItem
        {
            Name = "Item1",
            Description = "Test Item",
            Value = value,
            AppUserId = userId,
            PawnShopId = 1,  // Assuming a valid pawn shop id
            Status = PawnStatus.Pending,
            IsDeleted = false
        };
        _context.PawnItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }


    [Fact]
    public async Task CreateItemAsync_CreatesItemSuccessfully()
    {
        // Arrange
        var user = await CreateUserAsync(_userId, 5000);
        var model = new ItemViewModel
        {
            Name = "Item1",
            Description = "Test Item",
            Value = _itemValue,
            PawnShopId = 1
        };

        // Act
        var item = await _itemService.CreateItemAsync(model, _userId);

        // Assert
        Assert.NotNull(item);
        Assert.Equal(_userId, item.AppUserId);
        Assert.Equal(_itemValue, item.Value);
        Assert.Equal(PawnStatus.Pending, item.Status);
    }

    [Fact]
    public async Task GetPawnShopsAsync_ReturnsListOfPawnShops()
    {
        // Arrange
        var pawnShop1 = new PawnShop { Id = 1, Name = "Pawn Shop 1", City = "City 1", IsDeleted = false };
        var pawnShop2 = new PawnShop { Id = 2, Name = "Pawn Shop 2", City = "City 2", IsDeleted = false };
        _context.PawnShops.Add(pawnShop1);
        _context.PawnShops.Add(pawnShop2);
        await _context.SaveChangesAsync();

        // Act
        var pawnShops = await _itemService.GetPawnShopsAsync();

        // Assert
        Assert.Equal(2, pawnShops.Count);
        Assert.Contains(pawnShops, ps => ps.Value == "1" && ps.Text == "Pawn Shop 1");
        Assert.Contains(pawnShops, ps => ps.Value == "2" && ps.Text == "Pawn Shop 2");
    }

    [Fact]
    public async Task FindItemByIdAsync_ReturnsItemWhenFound()
    {
        // Arrange
        var item = await CreateItemAsync(_userId, _itemValue);

        // Act
        var foundItem = await _itemService.FindItemByIdAsync(item.Id);

        // Assert
        Assert.NotNull(foundItem);
        Assert.Equal(item.Id, foundItem.Id);
    }

    [Fact]
    public async Task UpdateItemStatusAsync_UpdatesStatusSuccessfully()
    {
        // Arrange
        var item = await CreateItemAsync(_userId, _itemValue);
        var newStatus = PawnStatus.Pawned;

        // Act
        await _itemService.UpdateItemStatusAsync(item.Id, newStatus);
        var updatedItem = await _itemService.FindItemByIdAsync(item.Id);

        // Assert
        Assert.Equal(newStatus, updatedItem.Status);
    }

    [Fact]
    public async Task ApproveItemAsync_ApprovesItemSuccessfully()
    {
        // Arrange
        var user = await CreateUserAsync(_userId, 5000);
        var adminUser = new AppUser { Id = _adminUserId, CurrencyAmount = 5000 };
        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();

        var item = await CreateItemAsync(_userId, _itemValue); 
        item.AppUserId = _userId; // Set the AppUserId for the pawn item 
        _context.PawnItems.Update(item);
        await _context.SaveChangesAsync();

        _userManagerMock.Setup(x => x.FindByIdAsync(_adminUserId)).ReturnsAsync(adminUser); 
        _userManagerMock.Setup(x => x.FindByIdAsync(_userId)).ReturnsAsync(user); 

        // Act
        var result = await _itemService.ApproveItemAsync(item.Id, adminUser);

        // Assert
        Assert.True(result); // The approval should succeed
        Assert.Equal(PawnStatus.Pawned, item.Status); // The status should be updated to "Pawned"
        Assert.Equal(4000, adminUser.CurrencyAmount); // Admin's currency amount should be reduced by the item value
        Assert.Equal(6000, user.CurrencyAmount); // User's currency amount should be increased by the item value
    }

    [Fact]
    public async Task RedeemItemAsync_RedeemsItemSuccessfully()
    {
        // Arrange
        var itemValue = 5000;  // The original value of the item
        var totalAmount = itemValue + (0.2m * itemValue); // Total amount = Item value + 20% tax

        
        var user = await CreateUserAsync("user123", 10000);  
        var adminUser = new AppUser
        {
            Id = "admin123",
            CurrencyAmount = 10000 
        };

        _context.Users.Add(adminUser);
        await _context.SaveChangesAsync();

        
        var item = new PawnItem
        {
            Id = 1,
            Name = "Test Item",  // Setting required Name property
            Description = "A test item for redeem", // Setting required Description property
            Value = itemValue,
            Status = PawnStatus.Pawned,
            AppUserId = user.Id,
            DueDate = DateTime.Now.AddMonths(1),
            PawnDate = DateTime.Now
        };

        _context.PawnItems.Add(item);
        await _context.SaveChangesAsync();

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _pawnShopAdminServiceMock.Setup(x => x.GetAdminUserAsync()).ReturnsAsync(adminUser);

        var itemService = new ItemService(_userManagerMock.Object, _pawnShopAdminServiceMock.Object, _context);

        // Act
        var result = await itemService.RedeemItemAsync(user, item);

        // Assert
        Assert.True(result); // Redeem should be successful
        Assert.Equal(10000 - totalAmount, user.CurrencyAmount); // User's currency should be deducted by the total amount (10000 - 6000)
        Assert.Equal(10000 + totalAmount, adminUser.CurrencyAmount); // Admin's currency should be updated (10000 + 6000)
        Assert.Equal(PawnStatus.Redeemed, item.Status); // Item status should be "Redeemed"
    }

    [Fact]
    public async Task DeleteItemAsync_DeletesItemSuccessfully()
    {
        // Arrange
        var item = await CreateItemAsync(_userId, _itemValue);

        // Act
        await _itemService.DeleteItemAsync(item);
        var deletedItem = await _itemService.FindItemByIdAsync(item.Id);

        // Assert
        Assert.NotNull(deletedItem);
        Assert.True(deletedItem.IsDeleted);
    }

    [Fact]
    public async Task GetBoughtItemsAsync_ReturnsBoughtItemsForUser()
    {
        // Arrange
        var user = await CreateUserAsync(_userId, 5000);
        var item = await CreateItemAsync(_userId, _itemValue);
        item.Status = PawnStatus.Sold;
        item.IsDeleted = true;

        _context.PawnItems.Update(item);
        await _context.SaveChangesAsync();

        // Act
        var boughtItems = await _itemService.GetBoughtItemsAsync(_userId);

        // Assert
        Assert.Single(boughtItems);
        Assert.Equal(PawnStatus.Sold, boughtItems[0].Status);
    }

}

