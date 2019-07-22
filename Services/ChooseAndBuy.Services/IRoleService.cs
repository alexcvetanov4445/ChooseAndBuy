namespace ChooseAndBuy.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ChooseAndBuy.Web.ViewModels.Administration.Roles;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IRoleService
    {
        Task<IEnumerable<UsersRoleViewModel>> GetUsersWithRole();

        Task<IEnumerable<SelectListItem>> GetAllRoles();

        Task<bool> ChangeUserRole(string username, string newRole);

        Task<bool> AddRole(string name);

        Task<bool> RemoveRole(string name);
    }
}
