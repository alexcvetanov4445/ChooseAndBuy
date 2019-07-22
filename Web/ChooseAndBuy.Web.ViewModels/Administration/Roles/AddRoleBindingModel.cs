namespace ChooseAndBuy.Web.ViewModels.Administration.Roles
{
    using System.ComponentModel.DataAnnotations;

    public class AddRoleBindingModel
    {
        [Display(Name = "Role")]
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string RoleName { get; set; }
    }
}
