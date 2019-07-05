namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class ReviewBindingModel
    {
        public string ProductId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name must be atleast {1} and less than {0} characters long.")]
        public string ClientFullName { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Comment must be atleast {1} and less than {0} characters long.")]
        public string Comment { get; set; }
    }
}
