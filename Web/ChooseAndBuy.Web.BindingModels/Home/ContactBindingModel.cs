namespace ChooseAndBuy.Web.BindingModels.Home
{
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class ContactBindingModel : IMapTo<ContactMessage>
    {
        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The field \"{0}\" is required.")]
        [StringLength(400, MinimumLength = 20, ErrorMessage = "The \"{0}\" must be a text with minimum length of {2} and maximum length of {1}.")]
        public string Message { get; set; }
    }
}
