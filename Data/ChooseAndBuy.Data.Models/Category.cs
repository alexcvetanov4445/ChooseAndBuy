namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Category
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.SubCategories = new HashSet<SubCategory>();
        }

        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}
