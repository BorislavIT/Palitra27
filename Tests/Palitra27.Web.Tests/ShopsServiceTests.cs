namespace Palitra27.Web.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Shop;
    using Xunit;

    public class ShopsServiceTests
    {
        [Fact]
        public void FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThoseProperties_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel("brandYaah", "categoryYaah", 1, 0M, 10M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.Equal(item.Brand.Name, model.Brand);
                Assert.Equal(item.Category.Name, model.Category);
                Assert.True(item.Price >= lowestPricedProduct.Price && item.Price <= model.PriceUpper);
            }
        }

        [Fact]
        public void FindWithNoBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithNoBrandAndCategoryAndPriceShouldReturnAllProductswithThoseProperties_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel(string.Empty, "categoryYaah", 1, 0M, 10M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.Equal(item.Category.Name, model.Category);
                Assert.True(item.Price >= lowestPricedProduct.Price && item.Price <= model.PriceUpper);
            }
        }

        [Fact]
        public void FindWithNoBrandAndCategoryAndNoPriceUpperShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithNoBrandAndCategoryAndNoPriceUpperShouldReturnAllProductswithThoseProperties_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel(string.Empty, "categoryYaah", 1, 0M, 0M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.Equal(item.Category.Name, model.Category);
                Assert.True(item.Price >= lowestPricedProduct.Price);
            }
        }

        [Fact]
        public void FindWithBrandAndNoCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithBrandAndNoCategoryAndPriceShouldReturnAllProductswithThoseProperties_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel("brandYaah", string.Empty, 1, 0M, 10M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.Equal(item.Brand.Name, model.Brand);
                Assert.True(item.Price >= lowestPricedProduct.Price && item.Price <= model.PriceUpper);
            }
        }

        [Fact]
        public void FindWithNoBrandAndNoCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithNoBrandAndNoCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedASCbyprice_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel(string.Empty, string.Empty, 1, 0M, 10M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.True(item.Price >= lowestPricedProduct.Price && item.Price <= model.PriceUpper);
            }
        }

        [Fact]
        public void FindWithNoBrandAndNoCategoryAndNoPriceUpperShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithNoBrandAndNoCategoryAndNoPriceUpperShouldReturnAllProductsOrderedASCbyprice_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel(string.Empty, string.Empty, 1, 0M, 10M, 12, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.True(item.Price >= lowestPricedProduct.Price);
            }
        }

        [Fact]
        public void FindWithNoBrandAndNoCategoryAndPriceUpperIs0ShouldReturnAllProductswithThosePropertiesOrderedAscendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithNoBrandAndNoCategoryAndPriceUpperIs0ShouldReturnAllProductswithThosePropertiesOrderedbypRIceasc")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel(string.Empty, string.Empty, 1, 0M, 0M, 0, "pASC");
            var result = shopService.Find(model);
            var lowestPricedProduct = result[0];

            foreach (var item in result)
            {
                Assert.True(item.Price >= lowestPricedProduct.Price);
            }
        }

        [Fact]
        public void FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedDescendingByPrice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedDescendingByPrice_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel("brandYaah", "categoryYaah", 1, 0M, 10M, 12, "pDESC");
            var result = shopService.Find(model);
            var highestPricedProduct = result[0];

            var sortedResult = result.OrderByDescending(x => x.Price).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i], sortedResult[i]);
            }
        }

        [Fact]
        public void FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAZByName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedAZByName_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel("brandYaah", "categoryYaah", 1, 0M, 10M, 12, "nAZ");
            var result = shopService.Find(model);

            var sortedResult = result.OrderBy(x => x.Name).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i], sortedResult[i]);
            }
        }

        [Fact]
        public void FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedZAByName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindWithBrandAndCategoryAndPriceShouldReturnAllProductswithThosePropertiesOrderedZAByName_Shop_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mapper = this.SetUpAutoMapper();

            var productsService = new ProductsService(dbContext, mapper);
            var shopService = new ShopService(dbContext, productsService, mapper);

            this.SeedDbWithProductsAndBrandsAndCategories(dbContext, "brandYaah", "categoryYaah", "productYaah");
            var products = productsService.FindAllProducts();
            var model = this.CreateShopViewModel("brandYaah", "categoryYaah", 1, 0M, 10M, 12, "nZA");
            var result = shopService.Find(model);

            var sortedResult = result.OrderByDescending(x => x.Name).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i], sortedResult[i]);
            }
        }

        private ShopViewModel CreateShopViewModel(string brand, string category, int page, decimal priceLower, decimal priceUpper, int show, string sorting)
        {
            return new ShopViewModel { Brand = brand, Category = category, Page = page, PriceLower = priceLower, PriceUpper = priceUpper, Show = show, Sorting = sorting };
        }

        private void SeedDbWithProductsAndBrandsAndCategories(ApplicationDbContext dbContext, string brandName, string categoryName, string productName)
        {
            var products = new List<Product>();
            var brands = new List<Brand>();
            var categories = new List<Category>();
            for (int i = 0; i < 4; i++)
            {
                var brand = new Brand { Name = $"brand{i}" };
                brands.Add(brand);
                var category = new Category { Name = $"category{i}" };
                categories.Add(category);
                products.Add(new Product { Brand = brand, Category = category, Name = $"productNamae{i}", Price = 1 + i });
            }

            for (int i = 0; i < 4; i++)
            {
                var brand = new Brand { Name = brandName };
                brands.Add(brand);
                var category = new Category { Name = categoryName };
                categories.Add(category);
                products.Add(new Product { Brand = brand, Category = category, Name = productName, Price = 5 + i });
            }

            for (int i = 0; i < 4; i++)
            {
                var brand = new Brand { Name = $"brandNamae{i}" };
                brands.Add(brand);
                var category = new Category { Name = $"categoryNamae{i}" };
                categories.Add(category);
                products.Add(new Product { Brand = brand, Category = category, Name = $"product{i}", Price = 9 + i });

            }

            dbContext.Brands.AddRange(brands);
            dbContext.Categories.AddRange(categories);
            dbContext.Products.AddRange(products);
            dbContext.SaveChanges();
        }

        private IMapper SetUpAutoMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            return mockMapper.CreateMapper();
        }
    }
}
