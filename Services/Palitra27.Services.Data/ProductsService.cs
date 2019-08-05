namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductsService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public ProductDTO GetOnlyProductById(string id)
        {
            var product = this.context.Products.Include(p => p.Category)
                                   .Include(x => x.Brand)
                                   .FirstOrDefault(x => x.Id == id);

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO Create(CreateProductBindingModel model)
        {
            var checkProduct = this.context.Products
              .FirstOrDefault(x => x.Name == model.ProductName);
            if (checkProduct != null)
            {
                return null;
            }

            var brand = this.context.Brands
                .FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories
                .FirstOrDefault(c => c.Name == model.Category);

            if (brand == null || category == null)
            {
                return null;
            }

            Product product = new Product()
            {
                Category = category,
                Price = model.Price,
                Name = model.ProductName,
                Brand = brand,
            };
            this.context.Products.Add(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO Create(CreateProductBindingModel model, IFormFile image)
        {
            var checkProduct = this.context.Products
              .FirstOrDefault(x => x.Name == model.ProductName);
            if (checkProduct != null)
            {
                return null;
            }

            byte[] arrOfImage = this.GetByteArrayFromImage(model.Image);
            string imreBase64Data = Convert.ToBase64String(arrOfImage);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

            var brand = this.context.Brands
                .FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories
                .FirstOrDefault(c => c.Name == model.Category);

            Product product = new Product()
            {
                Category = category,
                Price = model.Price,
                Name = model.ProductName,
                Brand = brand,
                Image = imgDataURL,
            };
            this.context.Products.Add(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO FindProductById(string productId)
        {
            var product = this.context.Products
             .Include(p => p.Reviews)
             .Include(p => p.Category)
             .Include(p => p.Brand)
             .FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                return null;
            }

            var reviews = this.context.Reviews
                .Where(r => r.ProductId == product.Id).ToList();

            product.Reviews = reviews;

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO EditProduct(ProductEditBindingModel model)
        {
            var product = this.context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == model.Id);

            var reviews = this.context.Reviews
              .Where(r => r.ProductId == product.Id)
              .ToList();

            product.Reviews = reviews;

            var category = this.context.Categories
                .FirstOrDefault(c => c.Name == model.Category);

            var brand = this.context.Brands
                .FirstOrDefault(b => b.Name == model.Brand);

            product.Name = model.Name;
            product.MiniDescription = model.MiniDescription;
            product.Price = model.Price;
            product.Category = category;
            product.Brand = brand;

            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public ReviewDTO AddReview(AddReviewBindingModel model, ApplicationUserDTO user)
        {
            var product = this.FindDomainProduct(model.Id);
            var review = new Review { UserName = user.UserName, ProductId = model.Id, Product = product, Message = model.Message, Stars = model.Stars, DateOfCreation = DateTime.UtcNow };

            this.context.Reviews.Add(review);
            this.context.SaveChanges();

            return this.mapper.Map<ReviewDTO>(review);
        }

        public ProductDTO EditDescription(EditDescriptionBindingModel model)
        {
            var product = this.FindDomainProduct(model.Id);
            product.Description = model.Description;
            this.context.Products.Update(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public Product FindDomainProduct(string id)
        {
            var product = this.context.Products
                .Include(p => p.Reviews)
               .Include(p => p.Category)
               .Include(p => p.Brand)
               .FirstOrDefault(p => p.Id == id);

            var reviews = this.context.Reviews
                .Where(r => r.ProductId == product.Id).ToList();

            product.Reviews = reviews;

            return product;
        }

        public ProductDTO EditSpecifications(EditSpecificationsBindingModel model)
        {
            var product = this.FindDomainProduct(model.Id);

            product.Height = model.Height;
            product.Weight = model.Weight;
            product.Width = model.Width;
            product.Depth = model.Depth;

            this.context.Products.Update(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public List<ProductDTO> GetAllProducts()
        {
            var products = this.context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Reviews)
                .ToList();

            return this.mapper.Map<List<ProductDTO>>(products);
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}
