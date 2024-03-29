﻿namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CityService : ICityService
    {
        private readonly ApplicationDbContext context;

        public CityService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllCities()
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
