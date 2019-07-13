namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICityService
    {
        IEnumerable<SelectListItem> GetAllCities();
    }
}
