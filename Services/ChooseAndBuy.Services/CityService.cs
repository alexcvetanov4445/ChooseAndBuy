namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChooseAndBuy.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CityService : ICityService
    {
        private readonly ApplicationDbContext context;

        public CityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<SelectListItem> GetAllCities()
        {
            var cities = this.context.Cities.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id,
                    }).ToList();

            return cities;
        }
    }
}
