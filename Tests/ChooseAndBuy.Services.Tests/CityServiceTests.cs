namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Extensions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CityServiceTests
    {
        [Fact]
        public async Task GetAllCities_WithSeededData_ShouldReturnCorrectData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            var context = new ApplicationDbContext(options);
            await this.SeedDataToDatabase(context);

            var cityService = new CityService(context);

            var expectedData = this.GetCities().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            var actualData = await cityService.GetAllCities();

            foreach (var city in actualData)
            {
                Assert.True(
                    expectedData.Any(c => c.Text == city.Text && c.Value == city.Value),
                    "Data returned by CityService.GetAllCities() is incorrect.");
            }
        }

        [Fact]
        public async Task GetAllCities_WithoutSeededData_ShouldReturnEmptyCollection()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                       .Options;

            var context = new ApplicationDbContext(options);

            var cityService = new CityService(context);

            var actualData = await cityService.GetAllCities();

            AssertExtensions.EmptyWithMessage(actualData, "CityService.GetAllCities does not return an empty collection.");
        }

        public async Task SeedDataToDatabase(ApplicationDbContext context)
        {
            var cities = this.GetCities();

            await context.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }

        public IEnumerable<City> GetCities()
        {
            var cities = new List<City>()
            {
                new City
                {
                    Name = "Sofia",
                    Postcode = "1000",
                    Id = "0d8d1504-9418-423f-accb-487b17615c99",
                },
                new City
                {
                    Name = "Plovdiv",
                    Postcode = "2000",
                    Id = "44f591c1-d4a3-493d-b20c-c74fc7ba4855",
                },
                new City
                {
                    Name = "Varna",
                    Postcode = "3000",
                    Id = "374474d0-6ff3-4d7f-a221-cc23d7475d8a",
                },
            };

            return cities;
        }
    }
}
