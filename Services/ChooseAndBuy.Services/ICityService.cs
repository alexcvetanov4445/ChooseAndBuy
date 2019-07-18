namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICityService
    {
        Task<IEnumerable<SelectListItem>> GetAllCities();
    }
}
