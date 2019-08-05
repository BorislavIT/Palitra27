namespace Palitra27.Web.Tests
{
    using System.Linq;

    using AutoMapper;
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
            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { BrandName = "Brand"});
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { CategoryName = "Category" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = 23,
                ProductName = "product",
            };

            var productDTO = productService.Create(productBindingModel);

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
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { CategoryName = "Category" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = 23,
                ProductName = "product",
            };

            var productDTO = productService.Create(productBindingModel);

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
            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { BrandName = "Brand" });
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { CategoryName = "Category" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = 23,
                ProductName = "product",
            };

            var productDTO = productService.Create(productBindingModel);

            var products = dbContext.Products.ToList();

            var product1 = products[0];
            var productFromService = productService.GetOnlyProductById(product1.Id);

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

            var productFromService = productService.GetOnlyProductById(id);

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
            var brand = brandService.CreateBrand(new ViewModels.Brands.CreateBrandBindingModel { BrandName = "Brand" });
            var category = categoriesService.CreateCategory(new ViewModels.Categories.CreateCategoryBindingModel { CategoryName = "Category" });

            var productBindingModel = new CreateProductBindingModel
            {
                Category = category.Name,
                Brand = brand.Name,
                Price = 23,
                ProductName = "product",
            };

            var productDTO = productService.Create(productBindingModel);

            var products = dbContext.Products.ToList();

            var product1 = products[0];
            var productFromService = productService.GetOnlyProductById(product1.Id);

            Assert.Equal("product", productFromService.Name);
        }
    }
}
