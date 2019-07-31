namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.BindingModels.Addresses;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class AddressServiceTests
    {
        [Fact]
        public async Task CreateAddress_ShouldCreateAddressToUser()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var addressService = new AddressService(context);

            await this.SeedTestUser(context);

            AddressCreateBindingModel model = new AddressCreateBindingModel
            {
                AddressText = "AddressText Test 123",
                FirstName = "Test",
                LastName = "Testing",
                PhoneNumber = "0001112333",
            };

            // Gets the user and creates the address using the model and the users Id
            var user = this.GetUser();
            await addressService.CreateAddress(model, user.Id);

            var resultAddress = await context.Addresses.FirstAsync();

            Assert.True(
                resultAddress.ApplicationUserId == user.Id
                && resultAddress.AddressText == model.AddressText,
                "AddressService.CreateAddress() does not create a user address correctly.");
        }

        [Fact]
        public async Task GetAllUserAddresses_WithExistingUser_ShouldReturnCorrectAddresses()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var addressService = new AddressService(context);

            // seeding a user first and then addresses with the users id for foreign key
            await this.SeedTestUser(context);
            await this.SeedUserAddresses(context);

            // gets the user id and his addresses
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
                "AddressService.GetAllUserAddresses does not return the expected count.");
        }

        [Fact]
        public async Task GetAllUserAddresses_WithNonExistingUser_ShouldReturnCorrectAddresses()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

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
                "AddressService.GetAllUserAddresses does not return the expected count.");
        }

        public async Task SeedTestUser(ApplicationDbContext context)
        {
            var user = this.GetUser();

            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();
        }

        public async Task SeedUserAddresses(ApplicationDbContext context)
        {
            var addresses = this.GetAddressesForUser();

            await context.Addresses.AddRangeAsync(addresses);

            await context.SaveChangesAsync();
        }

        public ApplicationUser GetUser()
        {
            ApplicationUser user = new ApplicationUser
            {
                Id = "827c74c1-46ab-489d-914e-431297f55a7b",
                UserName = "TestUsername",
            };

            return user;
        }

        public ICollection<Address> GetAddressesForUser()
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

        public DbContextOptions<ApplicationDbContext> ConfigureContextOptionsAndAutoMapper()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ErrorBindingModel).GetTypeInfo().Assembly);

            return options;
        }
    }
}
