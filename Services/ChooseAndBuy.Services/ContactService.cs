using System.Threading.Tasks;
using ChooseAndBuy.Data;
using ChooseAndBuy.Data.Models;
using ChooseAndBuy.Web.ViewModels.Home;

namespace ChooseAndBuy.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext context;

        public ContactService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddContactMessage(ContactBindingModel model)
        {
            var message = AutoMapper.Mapper.Map<ContactMessage>(model);

            await this.context.AddAsync(message);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }
    }
}
