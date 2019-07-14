namespace Palitra27.Services.Data
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext context;

        public ProductsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ProductBrand> FindAllBrands()
        {
            var brands = this.context.ProductsBrands;

            return brands;
        }

        public IQueryable<Category> FindAllCategories()
        {
            var categories = this.context.Categories;

            return categories;
        }

        public void Create(CreateProductBindingModel model)
        {
            var brand = this.context.ProductsBrands.FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            Product product = new Product() { Category = category, Image = model.Image, Price = model.Price, Name = model.ProductName, Brand = brand };
            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public Product FindProductById(string id)
        {
            var reviews = this.context.Reviews.Where(r => r.ProductId == id).ToList();
            foreach (var item in reviews)
            {
                var user = this.context.Users.FirstOrDefault(u => u.Id == item.UserId);
                item.User = user;
            }

            var product = this.context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);
            product.Reviews = reviews;

            return product;
        }

        public Product EditProduct(ProductEditBindingModel model)
        {
            var reviews = this.context.Reviews.Where(r => r.ProductId == model.Id).ToList();
            foreach (var item in reviews)
            {
                var user = this.context.Users.FirstOrDefault(u => u.Id == item.UserId);
                item.User = user;
            }

            var product = this.context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == model.Id);
            product.Reviews = reviews;

            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            var brand = this.context.ProductsBrands.FirstOrDefault(b => b.Name == model.Brand);

            product.Name = model.Name;
            product.MiniDescription = model.MiniDescription;
            product.Price = model.Price;
            product.Category = category;
            product.Brand = brand;

            this.context.SaveChanges();

            return product;
        }

        public Review AddReview(AddReviewBindingModel model, string userId)
        {
            var review = new Review { Message = model.Message, ProductId = model.Id, Stars = model.Stars, DateOfCreation = DateTime.Now, UserId = userId };

            this.context.Reviews.Add(review);
            this.context.SaveChanges();

            return review;
        }

        public Product EditDescription(EditDescriptionBindingModel model)
        {
            var product = this.FindProductById(model.Id);
            product.Description = model.Description;
            this.context.SaveChanges();
            return product;
        }

        public Product EditSpecifications(EditSpecificationsBindingModel model)
        {
            var product = this.FindProductById(model.Id);

            product.Height = model.Height;
            product.Weight = model.Weight;
            product.Width = model.Width;
            product.Depth = model.Depth;

            this.context.SaveChanges();

            return product;
        }
    }
}
