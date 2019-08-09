namespace ChooseAndBuy.Services.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AddressServiceTests
    {
        private ApplicationUser GetUser()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "827c74c1-46ab-489d-914e-431297f55a7b",
                UserName = "TestUsername",
            };

            return user;
        }

        private ICollection<Address> GetAddressesForUser()
        {
            var userId = this.GetUser().Id;

            var addresses = new List<Address>() {
                new Address
                {
                    AddressText = "FirstTestAddress",
                    ApplicationUserId = userId,
                    FirstName = "First",
                    LastName = "Last",
                },
                new Address
                {
                    AddressText = "SecondTestAddress",
                    ApplicationUserId = userId,
                    FirstName = "First",
                    LastName = "Last",
                },
            };

            return addresses;
        }

        private async Task SeedTestUser(ApplicationDbContext context)
        {
            var user = this.GetUser();

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
        }

        private async Task SeedUserAddresses(ApplicationDbContext context)
        {
            var addresses = this.GetAddressesForUser();

            await context.Addresses.AddRangeAsync(addresses);

            await context.SaveChangesAsync();
        }

        public AddressServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task CreateAddress_ShouldCreateAddressToUser()
        {
            string onFalseErrorMessage = "Method does not create a user address correctly.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var addressService = new AddressService(context);

            await this.SeedTestUser(context);

            AddressCreateBindingModel model = new AddressCreateBindingModel
            {
                AddressText = "AddressText Test 123",
                FirstName = "Test",
                LastName = "Testing",
                PhoneNumber = "0001112333",
            };

            // Gets the user and creates the address using the model and the user's Id
            var user = this.GetUser();
            await addressService.CreateAddress(model, user.Id);

            var resultAddress = await context.Addresses.FirstAsync();

            Assert.True(
                resultAddress.ApplicationUserId == user.Id
                && resultAddress.AddressText == model.AddressText,
                onFalseErrorMessage);
        }

        [Fact]
        public async Task GetAllUserAddresses_WithExistingUser_ShouldReturnCorrectAddresses()
        {
            string onCountDifferenceErrorMessage = "Method does not return the expected count.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var addressService = new AddressService(context);

            // seeding a user first and then addresses with the users id for foreign key
            await this.SeedTestUser(context);
            await this.SeedUserAddresses(context);

            // gets the user id
            var userId = this.GetUser().Id;

            var addresses = await addressService.GetAllUserAddresses(userId);

            var expectedUserAddressesCount = this.GetAddressesForUser().Count;
            var actualCount = 0;

            foreach (var address in addresses)
            {
                actualCount++;
            }

            AssertExtensions.EqualCountWithMessage(
                expectedUserAddressesCount,
                actualCount,
                onCountDifferenceErrorMessage);
        }

        [Fact]
        public async Task GetAllUserAddresses_WithNoSeededAddresses_ShouldReturnAnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "The method did not return an empty collection upon no seeded addresses.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var addressService = new AddressService(context);

            // seeding a user only
            await this.SeedTestUser(context);

            // gets the user id
            var userId = this.GetUser().Id;

            var addresses = await addressService.GetAllUserAddresses(userId);

            AssertExtensions.EmptyWithMessage(addresses, onNonEmptyCollectionErrorMessage);
        }

        [Fact]
        public async Task GetAllUserAddresses_WithNonExistingUser_ShouldReturnCorrectAddresses()
        {
            string onNonEmptyCollectionErrorMessage = "The method does not return the expected count.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var addressService = new AddressService(context);

            // passing a non-existing user Id
            var addresses = await addressService.GetAllUserAddresses("randomId");

            var expectedCount = 0;
            var actualCount = 0;

            foreach (var address in addresses)
            {
                actualCount++;
            }

            AssertExtensions.EqualCountWithMessage(
                expectedCount,
                actualCount,
                onNonEmptyCollectionErrorMessage);
        }
    }
}
