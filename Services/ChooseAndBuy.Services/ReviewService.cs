namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;

    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext context;

        public ReviewService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool AddReview(Review review)
        {
            this.context.Reviews.Add(review);
            this.context.SaveChanges();

            return true;
        }

        public IEnumerable<Review> GetReviewsForProduct(string productId)
        {
            var reviews = this.context.Reviews.Where(pr => pr.ProductId == productId);

            return reviews;
        }
    }
}
