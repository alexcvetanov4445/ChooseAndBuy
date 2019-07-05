namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using ChooseAndBuy.Data.Models;

    public interface IReviewService
    {
        IEnumerable<Review> GetReviewsForProduct(string productId);

        bool AddReview(Review review);
    }
}
