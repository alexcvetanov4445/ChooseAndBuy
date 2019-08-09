namespace ChooseAndBuy.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Xunit;

    public class CityServiceTests
    {
        private IEnumerable<City> GetCities()
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

        private async Task SeedDataToDatabase(ApplicationDbContext context)
        {
            var cities = this.GetCities();

            await context.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }

        public CityServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task GetAllCities_WithSeededData_ShouldReturnCorrectData()
        {
            string onFalseErrorMessage = "Data returned by the method is incorrect.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            await this.SeedDataToDatabase(context);

            var cityService = new CityService(context);

            var expectedData = this.GetCities().Select(x => new SelectListItem { Text = x.Name, Value = x.Id }).ToList();
            var actualData = await cityService.GetAllCities();

            foreach (var city in actualData)
            {
                Assert.True(
                    expectedData.Any(c => c.Text == city.Text && c.Value == city.Value),
                    onFalseErrorMessage);
            }
        }

        [Fact]
        public async Task GetAllCities_WithoutSeededData_ShouldReturnEmptyCollection()
        {
            string onNonEmptyCollectionErrorMessage = "CityService.GetAllCities does not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var cityService = new CityService(context);

            var actualData = await cityService.GetAllCities();

            AssertExtensions.EmptyWithMessage(actualData, onNonEmptyCollectionErrorMessage);
        }
    }
}
