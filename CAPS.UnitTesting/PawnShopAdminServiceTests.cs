using CAPS.Data;
using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CAPS.UnitTesting;

public class PawnShopAdminServiceTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;
    private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
    private readonly Mock<UserManager<AppUser>> _userManagerMock;

    public PawnShopAdminServiceTests()
    {
        
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())  
            .Options;

        // Create mocks for RoleManager and UserManager 
        _roleManagerMock = new Mock<RoleManager<IdentityRole>>(Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);
        _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
    }

    private async Task<ApplicationDbContext> CreateContextAsync()
    {
        var context = new ApplicationDbContext(_options);
        await context.Database.EnsureCreatedAsync(); // Create the database if not already created
        return context;
    }

    private async Task AddTestPawnShopAsync(ApplicationDbContext context)
    {
        var pawnShop = new PawnShop
        {
            Id = 1,
            Name = "Test Shop",
            City = "Test City",
            LocationUrl = "http://testshop.com",
            IsDeleted = false
        };

        context.PawnShops.Add(pawnShop);
        await context.SaveChangesAsync();
    }

    private async Task<AppUser> AddTestAdminUserAsync(ApplicationDbContext context)
    {
        var adminRole = new IdentityRole("Admin");
        context.Roles.Add(adminRole);
        await context.SaveChangesAsync();

        var user = new AppUser { UserName = "owner", FullName = "Owner" };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Admin")).ReturnsAsync(new List<AppUser> { user });

        return user;
    }

    [Fact]
    public async Task CreatePawnShopAsync_AddsNewPawnShop()
    {
        // Arrange
        using var context = await CreateContextAsync();
        var service = new PawnShopAdminService(context, _roleManagerMock.Object, _userManagerMock.Object);

        var model = new PawnShopsViewModel
        {
            Name = "New Shop",
            City = "New City",
            LocationUrl = "http://newshop.com"
        };

        // Act
        await service.CreatePawnShopAsync(model);

        // Assert
        var pawnShop = await context.PawnShops.FirstOrDefaultAsync(ps => ps.Name == "New Shop");
        Assert.NotNull(pawnShop);
        Assert.Equal("New Shop", pawnShop.Name);
        Assert.Equal("New City", pawnShop.City);
        Assert.Equal("http://newshop.com", pawnShop.LocationUrl);
    }

    [Fact]
    public async Task UpdatePawnShopAsync_UpdatesExistingPawnShop()
    {
        // Arrange
        using var context = await CreateContextAsync();
        await AddTestPawnShopAsync(context);

        var service = new PawnShopAdminService(context, _roleManagerMock.Object, _userManagerMock.Object);

        var model = new PawnShopsViewModel
        {
            Id = 1,
            Name = "Updated Shop",
            City = "Updated City",
            LocationUrl = "http://updatedshop.com"
        };

        // Act
        await service.UpdatePawnShopAsync(model);

        // Assert
        var pawnShop = await context.PawnShops.FirstOrDefaultAsync(ps => ps.Id == 1);
        Assert.NotNull(pawnShop);
        Assert.Equal("Updated Shop", pawnShop.Name);
        Assert.Equal("Updated City", pawnShop.City);
        Assert.Equal("http://updatedshop.com", pawnShop.LocationUrl);
    }

    [Fact]
    public async Task DeletePawnShopAsync_SoftDeletesPawnShop()
    {
        // Arrange
        using var context = await CreateContextAsync();
        await AddTestPawnShopAsync(context);

        var service = new PawnShopAdminService(context, _roleManagerMock.Object, _userManagerMock.Object);

        // Act
        await service.DeletePawnShopAsync(1);

        // Assert
        var pawnShop = await context.PawnShops.FirstOrDefaultAsync(ps => ps.Id == 1);
        Assert.NotNull(pawnShop);
        Assert.True(pawnShop.IsDeleted);
    }

    [Fact]
    public async Task GetPawnShopByIdAsync_ReturnsCorrectPawnShop()
    {
        // Arrange
        using var context = await CreateContextAsync();
        await AddTestPawnShopAsync(context);

        var service = new PawnShopAdminService(context, _roleManagerMock.Object, _userManagerMock.Object);

        // Act
        var result = await service.GetPawnShopByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Shop", result.Name);
    }

    [Fact]
    public async Task GetAdminUserAsync_ReturnsCorrectAdminUser()
    {
        // Arrange
        using var context = await CreateContextAsync(); 

        
        var adminRole = new IdentityRole("Admin"); 
        context.Roles.Add(adminRole);
        await context.SaveChangesAsync();  

        
        var user = new AppUser { UserName = "owner", FullName = "Owner" }; 
        context.Users.Add(user);
        await context.SaveChangesAsync();  

        
        _userManagerMock.Setup(um => um.GetUsersInRoleAsync("Admin"))  // get the users with the role admin
            .ReturnsAsync(new List<AppUser> { user });

       
        _roleManagerMock.Setup(rm => rm.FindByNameAsync("Admin"))  
            .ReturnsAsync(adminRole);

        
        var service = new PawnShopAdminService(context, _roleManagerMock.Object, _userManagerMock.Object);

        // Act
        var result = await service.GetAdminUserAsync();  

        // Assert
        Assert.NotNull(result);  // Ensure the result is not null
        Assert.Equal("Owner", result.FullName);  // Ensure the correct user is returned
    }
}
