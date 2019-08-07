namespace Palitra27.Web.Tests
{
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class ProductsServiceTests
    {
        [Fact]
        public void CreateProductShouldCreateProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { Name = "Brandyy" });
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { Name = "Categoryyy" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = "23",
                Name = "product",
            };

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);
            var products = dbContext.Products.ToList();

            Assert.Single(products);
            Assert.Equal(productDTO.Name, products.First().Name);
        }

        [Fact]
        public void CreateProductShouldntCreateProductIfInvalidBrandOrCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = new Brand { Id = "d", Name = "" };
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { Name = "Categoryy" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = "23",
                Name = "product",
            };

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);
            var products = dbContext.Products.ToList();

            Assert.Null(productDTO);
            Assert.Empty(products);
        }

        [Fact]
        public void GetProductByIdShouldReturnProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { Name = "Brandd" });
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { Name = "Categoryyyyy" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = "23",
                Name = "product",
            };

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);
            var products = dbContext.Products.ToList();

            var product1 = products[0];
            var productFromService = productService.FindProductById(product1.Id);

            Assert.Equal("product", productFromService.Name);
        }

        [Theory]
        [InlineData("adsasdgdagdagadasd")]
        [InlineData("asdasgdagdagaddasd")]
        public void GetProductByIdShouldReturnNullIfInvalidId(string id)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);

            var productFromService = productService.FindProductById(id);

            Assert.Null(productFromService);
        }

        [Fact]
        public void FindProductByIdShouldIncludeReviews()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviews_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { Name = "Braasdnd" });
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { Name = "Categoryasd" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = "23",
                Name = "product",
            };

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);
            var products = dbContext.Products.ToList();

            var product1 = products[0];
            var productFromService = productService.FindProductById(product1.Id);

            Assert.Equal("product", productFromService.Name);
        }
    }
}
