namespace ChooseAndBuy.Services
{
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.BindingModels.Home;

    public interface IContactService
    {
        Task<bool> AddContactMessage(ContactBindingModel model);
    }
}
