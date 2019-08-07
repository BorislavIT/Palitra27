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

        public ProductDTO Create(CreateProductBindingModel model, IFormFile image)
        {
            if (this.CheckIfProductExists(model))
            {
                return null;
            }

            var brand = this.FindBrandByName(model.Brand);
            var category = this.FindCategoryByName(model.Category);

            if (this.CheckIfCategoryOrBrandIsNull(category, brand))
            {
                return null;
            }

            var product = this.CreateProductByModelBrandAndCategory(model, brand, category);

            this.context.Products.Add(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO FindProductById(string id)
        {
            var product = this.FindDomainProductById(id);

            if (this.CheckIfProductIsNull(product))
            {
                return null;
            }

            this.FindAndSetProductReviews(product);

            return this.mapper.Map<ProductDTO>(product);
        }

        public ProductDTO EditProduct(ProductEditBindingModel model)
        {
            var product = this.FindDomainProductById(model.Id);

            if (this.CheckIfProductIsNull(product))
            {
                return null;
            }

            product = this.FindAndSetProductReviews(product);

            var category = this.FindCategoryByName(model.Category);

            var brand = this.FindBrandByName(model.Brand);

            if (this.CheckIfCategoryOrBrandIsNull(category, brand))
            {
                return null;
            }

            product = this.EditProductMain(model, product, category, brand);

            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public ReviewDTO AddReview(AddReviewBindingModel model, ApplicationUserDTO user)
        {
            var product = this.FindDomainProduct(model.Id);
            var review = this.CreateReviewByModelProductAndUser(model, user, product);

            this.context.Reviews.Add(review);
            this.context.SaveChanges();

            return this.mapper.Map<ReviewDTO>(review);
        }

        public ProductDTO EditDescription(EditDescriptionBindingModel model)
        {
            var product = this.FindDomainProductById(model.Id);

            if (this.CheckIfProductIsNull(product))
            {
                return null;
            }

            product.Description = model.Description;
            this.context.Products.Update(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public Product FindDomainProduct(string id)
        {
            var product = this.FindDomainProductById(id);

            if (this.CheckIfProductIsNull(product))
            {
                return null;
            }

            product = this.FindAndSetProductReviews(product);

            return product;
        }

        public ProductDTO EditSpecifications(EditSpecificationsBindingModel model)
        {
            var product = this.FindDomainProduct(model.Id);

            if (this.CheckIfProductIsNull(product))
            {
                return null;
            }

            product = this.EditProductSpecifications(model, product);

            this.context.Products.Update(product);
            this.context.SaveChanges();

            return this.mapper.Map<ProductDTO>(product);
        }

        public List<ProductDTO> FindAllProducts()
        {
            var products = this.FindAllDomainProducts();

            if (this.CheckIfProductsDoesntHaveProducts(products))
            {
                return null;
            }

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

        private bool CheckIfCategoryOrBrandIsNull(Category category, Brand brand)
        {
            if (category == null || brand == null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfProductIsNull(Product product)
        {
            if (product == null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfProductExists(CreateProductBindingModel model)
        {
            var product = this.context.Products
             .Where(x => x.Brand.IsDeleted == false && x.Category.IsDeleted == false)
             .FirstOrDefault(x => x.Name == model.Name);

            if (product != null)
            {
                return true;
            }

            return false;
        }

        private bool CheckIfProductsDoesntHaveProducts(List<Product> products)
        {
            if (products.Count == 0)
            {
                return true;
            }

            return false;
        }

        private Product CreateProductByModelBrandAndCategory(CreateProductBindingModel model, Brand brand, Category category)
        {
            Product product = new Product()
            {
                Category = category,
                Price = decimal.Parse(model.Price),
                Name = model.Name,
                Brand = brand,
                Image = this.ParseToImgDataURL(model.Image) ?? null,
            };

            return product;
        }

        private string ParseToImgDataURL(IFormFile image)
        {
            if (image == null)
            {
                return null;
            }

            byte[] arrOfImage = this.GetByteArrayFromImage(image);
            string imreBase64Data = Convert.ToBase64String(arrOfImage);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);

            return imgDataURL;
        }

        private Brand FindBrandByName(string name)
        {
            var brand = this.context.Brands
                .Where(x => x.IsDeleted == false)
               .FirstOrDefault(b => b.Name == name);

            return brand;
        }

        private Category FindCategoryByName(string name)
        {
            var category = this.context.Categories
                .Where(x => x.IsDeleted == false)
               .FirstOrDefault(c => c.Name == name);

            return category;
        }

        private Product FindDomainProductById (string id)
        {
            var product = this.context.Products
             .Include(p => p.Reviews)
             .Include(p => p.Category)
             .Include(p => p.Brand)
             .Where(x => x.Brand.IsDeleted == false && x.Category.IsDeleted == false)
             .FirstOrDefault(p => p.Id == id);

            return product;
        }

        private Product FindAndSetProductReviews(Product product)
        {
            var reviews = this.context.Reviews
                 .Where(r => r.ProductId == product.Id)
                 .ToList();

            product.Reviews = reviews;

            return product;
        }

        private Product EditProductSpecifications(EditSpecificationsBindingModel model, Product product)
        {
            product.Height = model.Height;
            product.Weight = model.Weight;
            product.Width = model.Width;
            product.Depth = model.Depth;

            return product;
        }

        private Product EditProductMain(ProductEditBindingModel model, Product product, Category category, Brand brand)
        {
            product.Name = model.Name;
            product.MiniDescription = model.MiniDescription;
            product.Price = decimal.Parse(model.Price);
            product.Category = category;
            product.Brand = brand;

            return product;
        }

        private Review CreateReviewByModelProductAndUser(AddReviewBindingModel model, ApplicationUserDTO user, Product product)
        {
            var review = new Review { UserName = user.UserName, ProductId = model.Id, Product = product, Message = model.Message, Stars = model.Stars, DateOfCreation = DateTime.UtcNow };

            return review;
        }

        private List<Product> FindAllDomainProducts()
        {
            var products = this.context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.Reviews)
                .Where(x => x.Brand.IsDeleted == false && x.Category.IsDeleted == false)
                .ToList();

            return products;
        }
    }
}
