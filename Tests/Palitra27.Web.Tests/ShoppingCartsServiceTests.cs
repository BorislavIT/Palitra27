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
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class ShoppingCartsServiceTests
    {
        [Fact]
        public void AddProductInShoppingCartShoudlAdd1Product()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductInShoppingCartShoudlAdd1Product_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);

            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 1);
        }

        [Fact]
        public void AddProductInShoppingCartShouldntAddProductIfInvalidProductId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductInShoppingCartShouldntAddProductIfInvalidProductId_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart("totallyLegitProductId", user.UserName);

            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 0);
        }

        [Fact]
        public void AddProductInShoppingCartShouldntAddProductIfInvalidUserName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductInShoppingCartShouldntAddProductIfInvalidProductId_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, "totallyLegitUsername");

            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 0);
        }

        [Fact]
        public void AddProductShouldEditQuantityIfAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddProductShouldEditQuantityIfAlreadyExists_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            var shoppingCartProductQuantity = dbContext.ShoppingCartProducts.FirstOrDefault().Quantity;
            Assert.True(shoppingCartProductQuantity == 1);

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            var shoppingCartProductQuantity1 = dbContext.ShoppingCartProducts.FirstOrDefault().Quantity;
            Assert.True(shoppingCartProductQuantity1 == 2);
        }

        [Fact]
        public void RemoveProductFromShoppingCartShouldRemoveProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveProductFromShoppingCartShouldRemoveProduct_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.RemoveProductFromShoppingCart(products[0].Id, user.UserName);
            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 0);
        }

        [Fact]
        public void RemoveProductFromShoppingCartShouldntRemoveProductIfInvalidProductId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveProductFromShoppingCartShouldntRemoveProductIfInvalidProductId_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.RemoveProductFromShoppingCart("totallyLegitProductId", user.UserName);
            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 1);
        }

        [Fact]
        public void RemoveProductFromShoppingCartShouldntRemoveProductIfInvalidUserName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveProductFromShoppingCartShouldntRemoveProductIfInvalidUserName_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.RemoveProductFromShoppingCart(products[0].Id, "totallyLegitUserName");
            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 1);
        }

        [Fact]
        public void CheckIfAnyProductsInShoppingCartByUsernameShouldReturnTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CheckIfAnyProductsInShoppingCartByUsernameShouldReturnTrue_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            var result = shoppingCartsService.AnyProducts(user.UserName);
            Assert.True(result);
        }

        [Fact]
        public void CheckIfAnyProductsInShoppingCartByUsernameShouldReturnFalseIfNoProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CheckIfAnyProductsInShoppingCartByUsernameShouldReturnNoIfNoProducts_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            user.ShoppingCartId = user.ShoppingCart.Id;
            var result = shoppingCartsService.AnyProducts(user.UserName);
            Assert.False(result);
        }

        [Fact]
        public void RemoveAllProductsFromShoppingCartShouldRemoveAllProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveAllProductsFromShoppingCartShouldRemoveAllProducts_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 1);

            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.RemoveAllProductsFromShoppingCart(user.UserName);
            var shoppingCartProductsCount1 = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount1 == 0);
        }

        [Fact]
        public void RemoveAllProductsFromShoppingCartShouldntRemoveAllProductsIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveAllProductsFromShoppingCartShouldntRemoveAllProductsIfInvalidUsername_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.RemoveAllProductsFromShoppingCart("asddsa");
            var shoppingCartProductsCount = dbContext.ShoppingCartProducts.Count();
            Assert.True(shoppingCartProductsCount == 1);
        }

        [Fact]
        public void EditProductQuantityInShoppingCartShouldEditProductQuantity()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductQuantityInShoppingCartShouldEditProductQuantity_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.EditProductQuantityInShoppingCart(products[0].Id, user.UserName, 2);
            var shoppingCartProductsQuantity = dbContext.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == user.ShoppingCartId).Quantity;
            Assert.True(shoppingCartProductsQuantity == 2);
        }

        [Fact]
        public void EditProductQuantityInShoppingCartShouldntEditProductQuantityIfInvalidProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductQuantityInShoppingCartShouldntEditProductQuantityIfInvalidProduct_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.EditProductQuantityInShoppingCart("totallyLegiProductId", user.UserName, 2);
            var shoppingCartProductsQuantity = dbContext.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == user.ShoppingCartId).Quantity;
            Assert.True(shoppingCartProductsQuantity == 1);
        }

        [Fact]
        public void EditProductQuantityInShoppingCartShouldntEditProductQuantityIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductQuantityInShoppingCartShouldntEditProductQuantityIfInvalidUsername_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.EditProductQuantityInShoppingCart(products[0].Id, "totallyLegitUsername", 2);
            var shoppingCartProductsQuantity = dbContext.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == user.ShoppingCartId).Quantity;
            Assert.True(shoppingCartProductsQuantity == 1);
        }

        [Fact]
        public void EditProductQuantityInShoppingCartShouldntEditProductQuantityIfQuantityLessThan0()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductQuantityInShoppingCartShouldntEditProductQuantityIfQuantityLessThan0_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.EditProductQuantityInShoppingCart(products[0].Id, "totallyLegitUsername", -4);
            var shoppingCartProductsQuantity = dbContext.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == user.ShoppingCartId).Quantity;
            Assert.True(shoppingCartProductsQuantity == 1);
        }

        [Fact]
        public void EditProductQuantityInShoppingCartShouldntEditProductQuantityIfQuantityIs0()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductQuantityInShoppingCartShouldntEditProductQuantityIfQuantityIs0_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            shoppingCartsService.EditProductQuantityInShoppingCart(products[0].Id, "totallyLegitUsername", 0);
            var shoppingCartProductsQuantity = dbContext.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == user.ShoppingCartId).Quantity;
            Assert.True(shoppingCartProductsQuantity == 1);
        }

        [Fact]
        public void FindAllShoppingCartProductsShouldReturnAllShoppingCartProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllShoppingCartProductsShouldReturnAllShoppingCartProducts_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            var shoppingCartProducts = shoppingCartsService.FindAllShoppingCartProducts(user.UserName);
            Assert.True(shoppingCartProducts.Count == 1);
        }

        [Fact]
        public void FindAllShoppingCartProductsShouldReturnNullIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllShoppingCartProductsShouldReturnNullIfInvalidUsername_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            var shoppingCartProducts = shoppingCartsService.FindAllShoppingCartProducts("totallyLegitUsername");
            Assert.Null(shoppingCartProducts);
        }

        [Fact]
        public void FindAllDomainShoppingCartProductsShouldReturnAllShoppingCartProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllDomainShoppingCartProductsShouldReturnAllShoppingCartProducts_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            var shoppingCartProducts = shoppingCartsService.FindAllDomainShoppingCartProducts(user.UserName);
            Assert.IsType<List<ShoppingCartProduct>>(shoppingCartProducts);
            Assert.True(shoppingCartProducts.Count == 1);
        }

        [Fact]
        public void FindAllDomainShoppingCartProductsShouldReturnNullIfInvalidUsername()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllDomainShoppingCartProductsShouldReturnNullIfInvalidUsername_ShoppingCarts_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            shoppingCartsService.AddProductInShoppingCart(products[0].Id, user.UserName);
            user.ShoppingCartId = user.ShoppingCart.Id;
            var shoppingCartProducts = shoppingCartsService.FindAllDomainShoppingCartProducts("totallyLegitUsername");
            Assert.Null(shoppingCartProducts);
        }

        private void SeedDbWithUserAndProduct(ApplicationDbContext dbContext, ProductsService productsService, Category category, Brand brand, IFormFile formfile)
        {
            for (int i = 0; i < 5; i++)
            {
                var shoppingCartId = Guid.NewGuid().ToString();
                productsService.Create(new CreateProductBindingModel { Brand = brand.Name, Category = category.Name, Name = Guid.NewGuid().ToString(), Price = "15" }, formfile);
                dbContext.Users.Add(new ApplicationUser { UserName = $"{i}", FavouriteList = new FavouriteList(), ShoppingCart = new ShoppingCart { Id = shoppingCartId, ShoppingCartProducts = new List<ShoppingCartProduct>() }, ShoppingCartId = shoppingCartId });
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
