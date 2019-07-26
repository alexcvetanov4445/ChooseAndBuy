namespace ChooseAndBuy.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ChooseAndBuy.Data;
    using ChooseAndBuy.Data.Models;
    using ChooseAndBuy.Web.Areas.Administration.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.Products;
    using ChooseAndBuy.Web.ViewModels.ShoppingCart;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;
        private readonly ICloudinaryService imageService;

        public ProductService(
            ApplicationDbContext context,
            ICloudinaryService imageService)
        {
            this.context = context;
            this.imageService = imageService;
        }

        public async Task<bool> AddProduct(CreateProductBindingModel model)
        {
            var product = AutoMapper.Mapper.Map<Product>(model);

            string uniqueFileName = await this.imageService.CreateImage(model.FormImage, model.Name);
            product.ImageName = uniqueFileName;

            await this.context.Products.AddAsync(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> HideProduct(string id)
        {
            var product = await this.context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            var result = 0;

            if (product.IsHidden)
            {
                product.IsHidden = false;
                result = await this.context.SaveChangesAsync();
                return result > 0;
            }

            product.IsHidden = true;

            result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> RecommendProduct(string id)
        {
            var product = await this.context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return false;
            }

            var result = 0;

            if (product.IsRecommended)
            {
                product.IsRecommended = false;
                result = await this.context.SaveChangesAsync();
                return result > 0;
            }

            product.IsRecommended = true;

            result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EditProduct(EditProductBindingModel model)
        {
            var product = await this.context.Products.SingleOrDefaultAsync(p => p.Id == model.Id);

            AutoMapper.Mapper.Map(model, product);

            this.context.Products.Update(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IEnumerable<TableProductViewModel>> GetAllProducts()
        {
            var products = await this.context
                .Products
                .Include(p => p.SubCategory)
                .ToListAsync();

            var result = AutoMapper.Mapper.Map<List<TableProductViewModel>>(products);

            return result;
        }

        public async Task<ProductDetailsViewModel> GetById(string id)
        {
            var product = await this.context.Products
                .Include(x => x.ShoppingCartProducts)
                .ThenInclude(x => x.Product)
                .Include(r => r.Reviews)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(pr => pr.Id == id);

            var result = AutoMapper.Mapper.Map<ProductDetailsViewModel>(product);

            return result;
        }

        public async Task<EditProductBindingModel> GetEditProductInfoById(string productId)
        {
            var product = await this.context.Products
                .Include(x => x.ShoppingCartProducts)
                .ThenInclude(x => x.Product)
                .Include(r => r.Reviews)
                .Include(c => c.SubCategory)
                .SingleOrDefaultAsync(pr => pr.Id == productId);

            var result = AutoMapper.Mapper.Map<EditProductBindingModel>(product);

            return result;
        }

        public async Task<string> GetIdByName(string productName)
        {
            var name = this.context
                .Products
                .SingleOrDefault(x => x.Name == productName)
                .Name;

            return name;
        }

        public async Task<IEnumerable<ProductViewModel>> GetProducts(string search, string subCategoryId, int sortBy)
        {
            var products = new List<Product>();
            var result = new List<ProductViewModel>();

            if (search != null)
            {
                products = this.GetSearchedProducts(search).ToList();
            }
            else if (subCategoryId != null)
            {
                products = this.GetProductsByCategory(subCategoryId).ToList();
            }
            else
            {
                products = this.context.Products.Where(pr => pr.IsRecommended == true && pr.IsHidden == false).ToList();
            }

            result = AutoMapper.Mapper.Map<List<ProductViewModel>>(products);

            switch (sortBy)
            {
                case 1: // price ascending
                    return result.OrderBy(p => p.Price).ToList();
                case 2: // price descending
                    return result.OrderByDescending(p => p.Price).ToList();
                case 3: // name ascending
                    return result.OrderBy(p => p.Name).ToList();
                case 4: // name descending
                    return result.OrderByDescending(p => p.Name).ToList();
            }

            return result;
        }

        public IEnumerable<Product> GetSearchedProducts(string search)
        {
            var searchNormalized = search.Split(new string[] { ".", ",", " " }, StringSplitOptions.RemoveEmptyEntries);

            var products = this.context
                .Products
                .Where(p => !p.IsHidden && searchNormalized.All(s => p.Name.ToLower().Contains(s.ToLower())));

            return products;
        }

        public async Task<bool> ProductExists(string name)
        {
            var result = await this.context.Products.SingleOrDefaultAsync(n => n.Name == name);

            if (result == null)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<RecommendedProductViewModel>> GetRecommendationProducts()
        {
            var products = this.context
                .Products
                .Where(p => p.IsRecommended && p.IsHidden == false)
                .Take(6);

            var result = AutoMapper.Mapper.Map<List<RecommendedProductViewModel>>(products);

            return result;
        }

        private IEnumerable<Product> GetProductsByCategory(string id)
        {
            var products = this.context
                .Products
                .Where(sc => sc.SubCategoryId == id && sc.IsHidden == false);

            return products;
        }

        public async Task<ShoppingCartProductViewModel> GetProductForCart(string id)
        {
            var product = this.context.Products.SingleOrDefault(p => p.Id == id);

            var result = AutoMapper.Mapper.Map<ShoppingCartProductViewModel>(product);

            // sets the quantity to 1, because that will be the first product of that type added to the cart
            // sets the total price to the product price for the same reason above
            result.Quantity = 1;
            result.TotalPrice = (double)result.Price;

            return result;
        }
    }
}
