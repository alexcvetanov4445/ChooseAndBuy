namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.BindingModels.Home;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ContactServiceTests
    {
        private ContactBindingModel GetMessageModel()
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

        public ContactServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddContactMessage_WithData_ShouldAddTheMessageToDatabase()
        {
            string onFalseErrorMessage = "The method returned a false statement.";
            string onNullErrorMessage = "Contact message not found in database.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var contactService = new ContactService(context);

            var messageModel = this.GetMessageModel();

            var methodResult = await contactService.AddContactMessage(messageModel);

            Assert.True(methodResult, onFalseErrorMessage);

            var contactMessageFromDb = await context.ContactMessages.FirstOrDefaultAsync();

            AssertExtensions.NotNullWithMessage(contactMessageFromDb, onNullErrorMessage);
        }
    }
}
