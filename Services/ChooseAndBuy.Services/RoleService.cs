namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.ViewModels.Administration.Roles;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RoleService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> AddRole(string name)
        {
            name = name.First().ToString().ToUpper() 
                + name.ToLower().Substring(1);

            var roleExists = await this.roleManager.RoleExistsAsync(name);

            if (roleExists)
            {
                return false;
            }

            var role = new ApplicationRole();

            role.Name = name;

            await this.roleManager.CreateAsync(role);

            return true;
        }

        public async Task<bool> RemoveRole(string name)
        {

            bool isNameValid = name != "Admin" && name != "User";

            bool roleExists = await this.roleManager.RoleExistsAsync(name);

            if (!isNameValid)
            {
                return false;
            }

            var role = await this.roleManager.FindByNameAsync(name);

            // makes all the users with the role return to "User" role and afterwards deletes the role.
            await this.ReturnUsersToDefaultRole(role.Name);

            await this.roleManager.DeleteAsync(role);

            return true;
        }

        public async Task<bool> ChangeUserRole(string username, string newRole)
        {
            var user = await this.userManager.FindByNameAsync(username);

            var currRole = await this.userManager.GetRolesAsync(user);

            await this.userManager.RemoveFromRolesAsync(user, currRole);

            await this.userManager.AddToRoleAsync(user, newRole);

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllRoles()
        {
            List<SelectListItem> roles = await this.roleManager
                .Roles
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Name,
                }).ToListAsync();

            return roles;
        }

        public async Task<IEnumerable<UsersRoleViewModel>> GetUsersWithRole()
        {
            var users = await this.context.Users.ToListAsync();
            List<UsersRoleViewModel> userModels = new List<UsersRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await this.userManager.GetRolesAsync(user);

                UsersRoleViewModel userViewModel = new UsersRoleViewModel
                {
                    Role = roles.SingleOrDefault(),
                    Username = user.UserName,
                    Email = user.Email,
                };

                userModels.Add(userViewModel);
            }

            return userModels;
        }

        private async Task<bool> ReturnUsersToDefaultRole(string role)
        {
            var usersWithRole = await this.userManager.GetUsersInRoleAsync(role);

            foreach (var user in usersWithRole)
            {
                await this.userManager.RemoveFromRoleAsync(user, role);

                await this.userManager.AddToRoleAsync(user, "User");
            }

            return true;
        }
    }
}
