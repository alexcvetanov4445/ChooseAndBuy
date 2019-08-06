namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;
    using ChooseAndBuy.Services.Tests.Extensions;
    using ChooseAndBuy.Web.BindingModels;
    using ChooseAndBuy.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class RoleServiceTests
    {
        [Fact]
        public async Task AddRole_WithGivenValidRoleName_ShouldCreateRoleSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            var validRoleName = "Moderator";
            var methodResult = await roleService.AddRole(validRoleName);

            Assert.True(methodResult, "The method returned false upon valid input data for creation");

            var roleExistInRoleStore = await roleManager.FindByNameAsync(validRoleName);

            AssertExtensions.NotNullWithMessage(roleExistInRoleStore, "The role was not added succesfully into the roleStore (returned null)");
        }

        [Fact]
        public async Task AddRole_WithGivenNullInput_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            string invalidRoleName = null;

            var methodResult = await roleService.AddRole(invalidRoleName);

            Assert.False(methodResult, "The method returned true upon invalid data input for creation.");
        }

        [Fact]
        public async Task AddRole_WithExistingRoleWithName_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            string roleName = "Moderator";
            ApplicationRole role = new ApplicationRole();
            role.Name = roleName;

            // Creating the role so it will exist before the service method is called
            await roleManager.CreateAsync(role);

            var methodResult = await roleService.AddRole(roleName);

            Assert.False(methodResult, "The method returned true upon existing role with same name for creation.");
        }

        [Fact]
        public async Task RemoveRole_WithGivenExistingRoleWithName_ShouldRemoveRoleSuccessfully()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            var existingRole = "Moderator";
            ApplicationRole role = new ApplicationRole();
            role.Name = existingRole;

            // Creating the role so it will exist before the service method is called
            await roleManager.CreateAsync(role);

            var methodResult = await roleService.RemoveRole(existingRole);

            Assert.True(methodResult, "The method returned false upon valid input data for removal.");

            var roleExistInRoleStore = await roleManager.FindByNameAsync(existingRole);

            AssertExtensions.NullWithMessage(roleExistInRoleStore, "The role was not removed succesfully from the roleStore.");
        }

        [Fact]
        public async Task RemoveRole_WithNonExistingRoleWithName_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            var nonExistingRole = "Moderator";

            var methodResult = await roleService.RemoveRole(nonExistingRole);

            Assert.False(methodResult, "The method returned true upon invalid input data for removal.");
        }

        [Fact]
        public async Task RemoveRole_WithGivenNullInput_ShouldReturnFalse()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            string invalidRoleName = null;

            var methodResult = await roleService.RemoveRole(invalidRoleName);

            Assert.False(methodResult, "The method returned true upon invalid input data for removal.");
        }

        [Fact]
        public async Task GetAllRoles_WithSeededRoles_ShouldReturnCorrectRoles()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            await this.SeedRoles(roleManager);

            var roleService = new RoleService(context, userManager, roleManager);

            var resultRoles = roleService.GetAllRoles();

            string[] expectedNames = new string[] { "Moderator", "Admin", "User" };

            Assert.True(resultRoles.Result.Any(x => expectedNames.Contains(x.Text)), "The returned roles are not correct.");
        }

        [Fact]
        public async Task GetAllRoles_WithNoSeededRoles_ShouldReturnAnEmptyCollection()
        {
            var options = this.ConfigureContextOptionsAndAutoMapper();

            var context = new ApplicationDbContext(options);

            var userManager = this.ConfigureUserManager(context);
            var roleManager = this.ConfigureRoleManager(context);

            var roleService = new RoleService(context, userManager, roleManager);

            var resultRoles = roleService.GetAllRoles();

            var expectedCount = 0;

            Assert.True(resultRoles.Result.Count() == expectedCount, "The method did not return an empty collection.");
        }

        public async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            ApplicationRole role = new ApplicationRole
            {
                Id = "moderatorId",
                Name = "Moderator",
            };

            await roleManager.CreateAsync(role);

            ApplicationRole role2 = new ApplicationRole
            {
                Id = "adminId",
                Name = "Admin",
            };

            await roleManager.CreateAsync(role2);

            ApplicationRole role3 = new ApplicationRole
            {
                Id = "userId",
                Name = "User",
            };

            await roleManager.CreateAsync(role3);
        }

        public async Task SeedUsersWithRoles(ApplicationDbContext context)
        {
            List<ApplicationUser> users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "FirstTestUserId",
                    UserName = "First",
                },
                new ApplicationUser
                {
                    Id = "SecondTestUserId",
                    UserName = "Second",
                },
                new ApplicationUser
                {
                    Id = "ThirdTestUserId",
                    UserName = "Third",
                },
            };

            List<IdentityUserRole<string>> userRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = "moderatorId",
                    UserId = "FirstTestUserId",
                },
                new IdentityUserRole<string>
                {
                    RoleId = "adminId",
                    UserId = "SecondTestUserId",
                },
                new IdentityUserRole<string>
                {
                    RoleId = "userId",
                    UserId = "ThirdTestUserId",
                },
            };

            await context.Users.AddRangeAsync(users);
            await context.UserRoles.AddRangeAsync(userRoles);
            await context.SaveChangesAsync();
        }

        public UserManager<ApplicationUser> ConfigureUserManager(ApplicationDbContext context)
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                    new Mock<IUserStore<ApplicationUser>>().Object,
                    new Mock<IOptions<IdentityOptions>>().Object,
                    new Mock<IPasswordHasher<ApplicationUser>>().Object,
                    new IUserValidator<ApplicationUser>[0],
                    new IPasswordValidator<ApplicationUser>[0],
                    new Mock<ILookupNormalizer>().Object,
                    new Mock<IdentityErrorDescriber>().Object,
                    new Mock<IServiceProvider>().Object,
                    new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

            return mockUserManager.Object;
        }

        public RoleManager<ApplicationRole> ConfigureRoleManager(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore, null, null, null, null);

            return roleManager;
        }

        public DbContextOptions<ApplicationDbContext> ConfigureContextOptionsAndAutoMapper()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly,
                typeof(ErrorBindingModel).GetTypeInfo().Assembly);

            return options;
        }
    }
}
