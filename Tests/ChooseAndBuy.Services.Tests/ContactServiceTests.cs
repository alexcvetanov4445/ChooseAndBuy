namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.BindingModels.Home;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ContactServiceTests
    {
        [Fact]
        public async Task AddContactMessage_WithData_ShouldAddTheMessageToDatabase()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var contactService = new ContactService(context);

            var messageModel = this.GetMessageModel();

            var methodResult = await contactService.AddContactMessage(messageModel);

            Assert.True(methodResult, "The method returned a false statement.");

            var contactMessageFromDb = await context.ContactMessages.FirstOrDefaultAsync();

            AssertExtensions.NotNullWithMessage(contactMessageFromDb, "Contact message not found in database.");
        }

        public ContactBindingModel GetMessageModel()
        {
            ContactBindingModel model = new ContactBindingModel
            {
                Email = "TestMail@test.bg",
                Message = "Test message for testing purposes",
                Name = "Test Testing",
                Subject = "Test Subject",
            };

            return model;
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
