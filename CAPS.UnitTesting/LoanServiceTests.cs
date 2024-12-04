using CAPS.Data.Data;
using CAPS.DataModels;
using CAPS.Global;
using CAPS.Services;
using CAPS.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CAPS.UnitTesting
{
    public class LoanServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IPawnShopAdminService> _pawnShopAdminServiceMock;
        private readonly ApplicationDbContext _context;

        private readonly string _userId = "user123";
        private readonly int _loanAmount = 1000;
        private readonly int _loanTerm = 12;       
        private readonly string _adminUserId = "admin123";
        private LoanService _loanService;  

        public LoanServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _userManagerMock = new Mock<UserManager<AppUser>>(
                new Mock<IUserStore<AppUser>>().Object, null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<IdentityRole>>(
                new Mock<IRoleStore<IdentityRole>>().Object, null, null, null, null);

            _pawnShopAdminServiceMock = new Mock<IPawnShopAdminService>();

            
            _loanService = new LoanService(_context, _userManagerMock.Object, _roleManagerMock.Object, _pawnShopAdminServiceMock.Object);
        }

        private async Task<(AppUser user, Loan approvedLoan, Loan declinedLoan, Loan deletedLoan)> SetupLoansAsync()
        {
            var user = await CreateUserAsync(_userId, 5000);  // Creating user with initial balance of 5000

            // Creating approved loan
            var approvedLoan = await CreateLoanAsync(_userId, _loanAmount, _loanTerm);
            approvedLoan.LoanStatus = LoanStatus.Aproved;
            _context.Loans.Update(approvedLoan);
            await _context.SaveChangesAsync();

            // Creating declined loan
            var declinedLoan = await CreateLoanAsync(_userId, _loanAmount, _loanTerm);
            declinedLoan.LoanStatus = LoanStatus.Declined;
            _context.Loans.Update(declinedLoan);
            await _context.SaveChangesAsync();

            // Creating deleted loan
            var deletedLoan = await CreateLoanAsync(_userId, _loanAmount, _loanTerm);
            deletedLoan.IsDeleted = true;
            _context.Loans.Update(deletedLoan);
            await _context.SaveChangesAsync();

            return (user, approvedLoan, declinedLoan, deletedLoan);
        }


        // Helper method to create a user
        private async Task<AppUser> CreateUserAsync(string userId, decimal currencyAmount)
        {
            var user = new AppUser
            {
                Id = userId,
                CurrencyAmount = currencyAmount
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // Helper method to create an admin user
        private async Task<AppUser> CreateAdminUserAsync(string adminUserId, decimal currencyAmount)
        {
            var adminUser = new AppUser
            {
                Id = adminUserId,
                CurrencyAmount = currencyAmount
            };
            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();
            return adminUser;
        }

        // Helper method to create a loan
        private async Task<Loan> CreateLoanAsync(string userId, decimal amount, int term)
        {
            var loan = new Loan
            {
                AppUserId = userId,
                Amount = amount,
                LoanTerm = term,
                LoanStatus = LoanStatus.Pending,
                IssuedDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(term)
            };
            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        [Fact]
        public async Task CreateLoanAsync_CreatesLoanSuccessfully()
        {
            
            // Act
            var loan = await _loanService.CreateLoanAsync(_userId, _loanAmount, _loanTerm);

            // Assert
            Assert.NotNull(loan); // Loan should not be null
            Assert.Equal(_userId, loan.AppUserId); // UserId should match
            Assert.Equal(_loanAmount, loan.Amount); // Loan amount should match
            Assert.Equal(_loanTerm, loan.LoanTerm); // Loan term should match
            Assert.Equal(LoanStatus.Pending, loan.LoanStatus); // Loan should be pending by default
        }
        [Fact]
        public async Task ApproveLoanAsync_ApprovesLoanSuccessfully()
        {
            
           

            var user = await CreateUserAsync(_userId, 5000);  // Start with 5000
            var adminUser = await CreateAdminUserAsync(_adminUserId, 10000);  // Start with 10000
            var loan = await CreateLoanAsync(_userId, _loanAmount, _loanTerm);

            // Setup mocks for UserManager and PawnShopAdminService
            _userManagerMock.Setup(x => x.FindByIdAsync(_userId)).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.FindByIdAsync(_adminUserId)).ReturnsAsync(adminUser);
            _pawnShopAdminServiceMock.Setup(x => x.GetAdminUserAsync()).ReturnsAsync(adminUser);

            // Act
            var result = await _loanService.ApproveLoanAsync(loan.Id, _adminUserId);

            // Assert
            Assert.True(result); // Loan should be approved
            Assert.Equal(6000, user.CurrencyAmount); // User's currency amount should increase by loan amount (5000 + 1000)
            Assert.Equal(9000, adminUser.CurrencyAmount); // Admin's currency amount should decrease by loan amount (10000 - 1000)
            Assert.Equal(LoanStatus.Aproved, loan.LoanStatus); // Loan status should be "Approved"
        }


        [Fact]
        public async Task PayLoanAsync_PaysLoanSuccessfully()
        {
            // Arrange
           
            var paymentAmount = 1200;
           

          
            var loan = await CreateLoanAsync(_userId, _loanAmount, _loanTerm);
            var user = await CreateUserAsync(_userId, 1500);

            var adminUser = new AppUser
            {
                Id = "admin123",
                CurrencyAmount = 10000
            };
            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();

            
            _userManagerMock.Setup(x => x.FindByIdAsync(_userId)).ReturnsAsync(user);

           
            _pawnShopAdminServiceMock.Setup(x => x.GetAdminUserAsync()).ReturnsAsync(adminUser);

            var payViewModel = new PayViewModel { LoanId = loan.Id };

            // Act
            var result = await _loanService.PayLoanAsync(payViewModel, paymentAmount, _userId);

            // Assert
            Assert.True(result); // Payment should be successful
            Assert.Equal(300, user.CurrencyAmount); // User should have deducted the total due
            Assert.Equal(11200, adminUser.CurrencyAmount); // Admin's balance should be updated
            Assert.Equal(LoanStatus.PaidOff, loan.LoanStatus); // Loan status should be paid off
        }

        [Fact]
        public async Task DeclineLoanAsync_DeclinesLoanSuccessfully()
        {
            // Arrange
            var (user, approvedLoan, declinedLoan, deletedLoan) = await SetupLoansAsync();

            // Act
            var result = await _loanService.DeclineLoanAsync(declinedLoan.Id);

            // Assert
            Assert.True(result); // Loan should be declined
            var declinedLoanFromDb = await _context.Loans.FindAsync(declinedLoan.Id);
            Assert.Equal(LoanStatus.Declined, declinedLoanFromDb.LoanStatus); // Loan status should be "Declined"
        }

        [Fact]
        public async Task DeleteLoanAsync_DeletesLoanSuccessfully()
        {
            // Arrange
            var (user, approvedLoan, declinedLoan, deletedLoan) = await SetupLoansAsync();

            // Act
            var result = await _loanService.DeleteLoanAsync(deletedLoan.Id);

            // Assert
            Assert.True(result); // Loan should be marked as deleted
            var deletedLoanFromDb = await _context.Loans.FindAsync(deletedLoan.Id);
            Assert.True(deletedLoanFromDb.IsDeleted); // Loan's IsDeleted flag should be true
        }

        [Fact]
        public async Task FindAllApprovedLoansAsync_FindsApprovedLoansSuccessfully()
        {
            // Arrange
            var (user, approvedLoan, declinedLoan, deletedLoan) = await SetupLoansAsync();

            // Act
            var loans = await _loanService.FindAllApprovedLoansAsync(_userId);

            // Assert
            Assert.Single(loans); // Only one loan should be returned, which is approved
            Assert.Equal(LoanStatus.Aproved, loans[0].LoanStatus); // Loan status should be "Approved"
        }

        [Fact]
        public async Task FindAllDeclinedLoansAsync_FindsDeclinedLoansSuccessfully()
        {
            // Arrange
            var (user, approvedLoan, declinedLoan, deletedLoan) = await SetupLoansAsync();

            // Act
            var loans = await _loanService.FindAllDeclinedLoansAsync(_userId);

            // Assert
            Assert.Single(loans); // Only one loan should be returned, which is declined
            Assert.Equal(LoanStatus.Declined, loans[0].LoanStatus); // Loan status should be "Declined"
        }
    }
}

