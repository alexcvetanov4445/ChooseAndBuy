namespace ChooseAndBuy.Data.Models
{
    using System;

    public class Image
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public byte[] Img { get; set; }

        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
