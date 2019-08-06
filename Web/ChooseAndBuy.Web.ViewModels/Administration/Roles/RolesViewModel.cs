namespace ChooseAndBuy.Web.ViewModels.Administration.Roles
{
    using System.Collections.Generic;

    using ChooseAndBuy.Web.BindingModels.Administration.Roles;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class RolesViewModel
    {
        public RolesViewModel()
        {
            this.RolesModel = new UserRoleBindingModel();
            this.AddModel = new AddRoleBindingModel();
            this.RemoveModel = new RemoveRoleBindingModel();
        }

        public IEnumerable<SelectListItem> Roles { get; set; }

        public IEnumerable<UsersRoleViewModel> UsersInfo { get; set; }

        public UserRoleBindingModel RolesModel { get; set; }

        public AddRoleBindingModel AddModel { get; set; }

        public RemoveRoleBindingModel RemoveModel { get; set; }
    }
}
