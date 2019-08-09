namespace ChooseAndBuy.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Tests.Common;
    using ChooseAndBuy.Services.Tests.Extensions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class RoleServiceTests
    {
        private async Task SeedRoles(RoleManager<ApplicationRole> roleManager)
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

        private async Task SeedUsersWithRoles(ApplicationDbContext context)
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

        private RoleManager<ApplicationRole> ConfigureRoleManagerWithRoleStore(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore, null, null, null, null);

            return roleManager;
        }

        public RoleServiceTests()
        {
            MapperInitializer.InitializeMapper();
        }

        [Fact]
        public async Task AddRole_WithGivenValidRoleName_ShouldCreateRoleSuccessfully()
        {
            string onFalseErrorMessage = "The method returned false upon valid input data for creation";
            string onNullErrorMessage = "The role was not added succesfully into the roleStore (returned null)";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            var validRoleName = "Moderator";
            var methodResult = await roleService.AddRole(validRoleName);

            Assert.True(methodResult, onFalseErrorMessage);

            var roleExistInRoleStore = await roleManager.FindByNameAsync(validRoleName);

            AssertExtensions.NotNullWithMessage(roleExistInRoleStore, onNullErrorMessage);
        }

        [Fact]
        public async Task AddRole_WithGivenNullInput_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid data input for creation.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            string invalidRoleName = null;

            var methodResult = await roleService.AddRole(invalidRoleName);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task AddRole_WithExistingRoleWithName_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon existing role with same name for creation.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            string roleName = "Moderator";
            ApplicationRole role = new ApplicationRole();
            role.Name = roleName;

            // Creating the role so it will exist before the service method is called
            await roleManager.CreateAsync(role);

            var methodResult = await roleService.AddRole(roleName);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task RemoveRole_WithGivenExistingRoleWithName_ShouldRemoveRoleSuccessfully()
        {
            string onNotNullErrorMessage = "The role was not removed succesfully from the roleStore.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            var existingRole = "Moderator";
            ApplicationRole role = new ApplicationRole();
            role.Name = existingRole;

            // Creating the role so it will exist before the service method is called
            await roleManager.CreateAsync(role);

            var methodResult = await roleService.RemoveRole(existingRole);

            Assert.True(methodResult, "The method returned false upon valid input data for removal.");

            var roleExistInRoleStore = await roleManager.FindByNameAsync(existingRole);

            AssertExtensions.NullWithMessage(roleExistInRoleStore, onNotNullErrorMessage);
        }

        [Fact]
        public async Task RemoveRole_WithNonExistingRoleWithName_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid input data for removal.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            var nonExistingRole = "Moderator";

            var methodResult = await roleService.RemoveRole(nonExistingRole);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task RemoveRole_WithGivenNullInput_ShouldReturnFalse()
        {
            string onTrueErrorMessage = "The method returned true upon invalid input data for removal.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            string invalidRoleName = null;

            var methodResult = await roleService.RemoveRole(invalidRoleName);

            Assert.False(methodResult, onTrueErrorMessage);
        }

        [Fact]
        public async Task GetAllRoles_WithSeededRoles_ShouldReturnCorrectRoles()
        {
            string onFalseErrorMessage = "The returned roles are not correct.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            await this.SeedRoles(roleManager);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            var resultRoles = roleService.GetAllRoles();

            string[] expectedNames = new string[] { "Moderator", "Admin", "User" };

            Assert.True(resultRoles.Result.Any(x => expectedNames.Contains(x.Text)), onFalseErrorMessage);
        }

        [Fact]
        public async Task GetAllRoles_WithNoSeededRoles_ShouldReturnAnEmptyCollection()
        {
            string onFalseErrorMessage = "The method did not return an empty collection.";

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();

            var userManager = UserManagerInitializer.InitializeMockedUserManager();
            var roleManager = this.ConfigureRoleManagerWithRoleStore(context);

            var roleService = new RoleService(context, userManager.Object, roleManager);

            var resultRoles = roleService.GetAllRoles();

            var expectedCount = 0;

            Assert.True(resultRoles.Result.Count() == expectedCount, onFalseErrorMessage);
        }
    }
}
