namespace ChooseAndBuy.Services.Tests.Common
{
    using System;

    using ChooseAndBuy.Data;
    using Microsoft.EntityFrameworkCore;

    public static class ApplicationDbContextInMemoryFactory
    {
        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            return new ApplicationDbContext(options);
        }
    }
}
