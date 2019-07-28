namespace ChooseAndBuy.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ChooseAndBuy.Services;
    using ChooseAndBuy.Web.BindingModels.Administration.Roles;
    using ChooseAndBuy.Web.ViewModels.Administration.Roles;
    using Microsoft.AspNetCore.Mvc;

    public class RolesController : AdministrationController
    {
        private readonly IRoleService roleService;

        public RolesController(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        public async Task<IActionResult> Index(string statusMessage)
        {
            var users = await this.roleService.GetUsersWithRole();
            var roles = await this.roleService.GetAllRoles();

            RolesViewModel model = new RolesViewModel
            {
                UsersInfo = users,
                Roles = roles,
            };

            this.ViewData["message"] = statusMessage;

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(UserRoleBindingModel rolesModel)
        {
            // finds the user and deletes his current role, afterwards sets the new role.
            // only one role is allowed by user.
            await this.roleService.ChangeUserRole(rolesModel.Username, rolesModel.Role);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleBindingModel addModel)
        {
            // Adds a new role different than the existing ones
            var isAdded = await this.roleService.AddRole(addModel.RoleName);

            var message = isAdded ? "Successfully added role."
                : "Role already exists.";

            return this.RedirectToAction("Index", new { statusMessage = message });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RemoveRoleBindingModel removeModel)
        {
            // removes a role and makes all the users with it returned to "user" role
            // "User" and "Admin" roles are unable to be deleted
            var isRemoved = await this.roleService.RemoveRole(removeModel.RoleName);

            var message = isRemoved ? "Successfully removed role."
                : "Failed to remove role. (Note: \"Admin\" and \"User\" roles cannot be removed!)";

            return this.RedirectToAction("Index", new { statusMessage = message });
        }
    }
}