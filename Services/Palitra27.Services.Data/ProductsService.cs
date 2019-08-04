namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Common.DtoModels.Product;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ProductsService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IQueryable<ProductBrand> FindAllBrands()
        {
            var brands = this.context.ProductsBrands;

            return brands;
        }

        public ProductDTO GetOnlyProductById(string id)
        {
            var product = this.context.Products.Include(p => p.Category)
                                   .Include(x => x.Brand)
                                   .FirstOrDefault(x => x.Id == id);

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public IQueryable<Category> FindAllCategories()
        {
            var categories = this.context.Categories;

            return categories;
        }

        public ProductDTO Create(CreateProductBindingModel model)
        {
            var brand = this.context.ProductsBrands.FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            Product product = new Product() { Category = category, Price = model.Price, Name = model.ProductName, Brand = brand };
            this.context.Products.Add(product);
            this.context.SaveChanges();

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public ProductDTO FindProductById(string id)
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

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public ProductDTO EditProduct(ProductEditBindingModel model)
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

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public Review AddReview(AddReviewBindingModel model, string userId)
        {
            var review = new Review { Message = model.Message, ProductId = model.Id, Stars = model.Stars, DateOfCreation = DateTime.Now, UserId = userId };

            this.context.Reviews.Add(review);
            this.context.SaveChanges();

            return review;
        }

        public ProductDTO EditDescription(EditDescriptionBindingModel model)
        {
            var product = this.FindDomainProduct(model.Id);
            product.Description = model.Description;
            this.context.Products.Update(product);
            this.context.SaveChanges();
            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public Product FindDomainProduct(string id)
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

        public ProductDTO EditSpecifications(EditSpecificationsBindingModel model)
        {
            var product = this.FindDomainProduct(model.Id);

            product.Height = model.Height;
            product.Weight = model.Weight;
            product.Width = model.Width;
            product.Depth = model.Depth;

            this.context.Products.Update(product);
            this.context.SaveChanges();

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        public ProductDTO Create(CreateProductBindingModel model, IFormFile image)
        {
            byte[] asd = this.GetByteArrayFromImage(model.Image);
            string imreBase64Data = Convert.ToBase64String(asd);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            var brand = this.context.ProductsBrands.FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            Product product = new Product() { Category = category, Price = model.Price, Name = model.ProductName, Brand = brand, Image = imgDataURL };
            this.context.Products.Add(product);
            this.context.SaveChanges();

            var dTO = this.mapper.Map<ProductDTO>(product);
            return dTO;
        }

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        public IEnumerable<ProductDTO> GetAllProducts()
        {
            var products = this.context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Reviews)
                .ToList();
            var dTOList = new List<ProductDTO>();
            foreach (var item in products)
            {
                var dTO = this.mapper.Map<ProductDTO>(item);
                dTOList.Add(dTO);
            }
            return dTOList;
        }

        public List<ProductDTO> FindAllProductsByQuery(string query)
        {
            var products = this.context.Products;

            var queryProducts = products.Where(x => x.Name.Contains(query)).Take(5).ToList();

            var dTOList = new List<ProductDTO>();
            foreach (var item in queryProducts)
            {
                var dTO = this.mapper.Map<ProductDTO>(item);
                dTOList.Add(dTO);
            }

            return dTOList;
        }
    }
}
