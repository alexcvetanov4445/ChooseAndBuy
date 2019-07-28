namespace ChooseAndBuy.Web.BindingModels.Administration.Roles
{
    using System.ComponentModel.DataAnnotations;

    public class UserRoleBindingModel
    {
        public string Username { get; set; }

        [Required]
        public string Role { get; set; }

    }
}
