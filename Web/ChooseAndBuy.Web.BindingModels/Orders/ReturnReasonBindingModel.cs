using System.ComponentModel.DataAnnotations;

namespace ChooseAndBuy.Web.BindingModels.Orders
{
    public class ReturnReasonBindingModel
    {
        public string OrderId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "The field must be a text with minimum length of {2} and maximum length of {1}.")]
        public string ReturnReason { get; set; }
    }
}
