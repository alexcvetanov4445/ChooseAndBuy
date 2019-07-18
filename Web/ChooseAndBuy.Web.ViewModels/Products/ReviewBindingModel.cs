﻿namespace ChooseAndBuy.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Services.Mapping;

    public class ReviewBindingModel : IMapTo<Review>
    {
        public string ProductId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Please rate the product.")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name must be atleast {1} and less than {0} characters long.")]
        public string ClientFullName { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 10, ErrorMessage = "Comment must be atleast {1} and less than {0} characters long.")]
        public string Comment { get; set; }
    }
}
