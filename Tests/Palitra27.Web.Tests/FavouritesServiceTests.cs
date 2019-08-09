namespace Palitra27.Web.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class FavouritesServiceTests
    {
        [Fact]
        public void AddProductShouldAddProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductShouldAddProduct_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = products[0];
            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                User = user,
                UserId = user.Id,
                Id = favouriteListId,
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            favouritesService.AddProduct(product.Id, user.UserName);

            Assert.Equal(1, user.FavouriteList.FavouriteProducts.Count);
        }

        [Fact]
        public void AddProductShouldntAddProductIfProductIsInvalid()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductShouldntAddProductIfProductIsInvalid_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = new ProductDTO { Id = string.Empty };
            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");

            dbContext.Users.Update(user);
            dbContext.SaveChanges();

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                User = user,
                UserId = user.Id,
                Id = favouriteListId,
                FavouriteProducts = new List<FavouriteProduct>(),
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            favouritesService.AddProduct(product.Id, user.UserName);

            Assert.Equal(0, user.FavouriteList.FavouriteProducts.Count);
        }

        [Fact]
        public void AddProductShouldntAddProductIfProductIsAlreadyIn()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductShouldntAddProductIfProductIsAlreadyIn_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = products[0];
            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                User = user,
                UserId = user.Id,
                Id = favouriteListId,
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            favouritesService.AddProduct(product.Id, user.UserName);
            favouritesService.AddProduct(product.Id, user.UserName);

            Assert.Equal(1, user.FavouriteList.FavouriteProducts.Count);
        }

        [Fact]
        public void AddProductShouldntAddProductIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductShouldntAddProductIfInvalidUsername_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = products[0];

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                UserId = "totallyLegitUserId",
                Id = favouriteListId,
                FavouriteProducts = new List<FavouriteProduct>(),
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            favouritesService.AddProduct(product.Id, "totallyLegitUsername");

            Assert.Empty(dbContext.FavouriteLists.FirstOrDefault(x => x.UserId == "totallyLegitUserId").FavouriteProducts);
        }

        [Fact]
        public void AllFavouriteProductsShouldReturnNullIfEmpty()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AllFavouriteProductsShouldReturnNullIfEmpty_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = products[0];
            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                User = user,
                UserId = user.Id,
                Id = favouriteListId,
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            Assert.Empty(favouritesService.AllFavouriteProducts(user.UserName));
        }

        [Fact]
        public void AllFavouriteProductsShouldReturnAllFavouriteProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AllFavouriteProductsShouldReturnAllFavouriteProducts_FavouriteList_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var products = productsService.FindAllProducts();
            var product = products[0];
            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");

            var favouriteListId = Guid.NewGuid().ToString();
            var favouriteList = new FavouriteList
            {
                User = user,
                UserId = user.Id,
                Id = favouriteListId,
            };

            dbContext.FavouriteLists.Add(favouriteList);
            dbContext.SaveChanges();

            favouritesService.AddProduct(product.Id, user.UserName);

            Assert.Single(favouritesService.AllFavouriteProducts(user.UserName));
        }

        private void SeedDbWithUserAndProduct(ApplicationDbContext dbContext, ProductsService productsService, Category category, Brand brand, IFormFile formfile)
        {
            for (int i = 0; i < 5; i++)
            {
                productsService.Create(new CreateProductBindingModel { Brand = brand.Name, Category = category.Name, Name = Guid.NewGuid().ToString(), Price = "15" }, formfile);
                dbContext.Users.Add(new ApplicationUser { UserName = $"{i}", FavouriteList = new FavouriteList() });
                dbContext.SaveChanges();
            }
        }

        private void SeedDbWithCategories(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Categories.Add(new Category { Name = Guid.NewGuid().ToString().Substring(0, 15) });
                dbContext.SaveChanges();
            }
        }

        private void SeeDbdWithBrands(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Brands.Add(new Brand { Name = Guid.NewGuid().ToString().Substring(0, 15) });
                dbContext.SaveChanges();
            }
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
