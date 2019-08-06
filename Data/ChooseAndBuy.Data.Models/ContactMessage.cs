namespace ChooseAndBuy.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ContactMessage
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime SentDate { get; set; }
    }
}
