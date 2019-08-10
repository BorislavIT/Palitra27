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
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.Enums;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Orders;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class OrdersServiceTests
    {
        [Fact]
        public void CreateOrderShouldCreateOrder()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateOrderShouldCreateOrder_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithCountries(dbContext);
            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));

            Assert.True(dbContext.Orders.Count() == 1);
        }

        [Fact]
        public void CreateOrderShouldntCreateOrderIfNoPrductsInShoppingCart()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateOrderShouldntCreateOrderIfNoPrductsInShoppingCart_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);
            this.SeedDbWithCountries(dbContext);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));

            Assert.True(dbContext.Orders.Count() == 0);
        }

        [Fact]
        public void FindUserOrderByIdShouldReturnUserOrder()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindUserOrderByIdShouldReturnUserOrder_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);
            this.SeedDbWithCountries(dbContext);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));
            var order = ordersService.FindUserOrderById(orderId, user.UserName);

            Assert.NotNull(dbContext.Orders.FirstOrDefault(x => x.Id == order.Id));
        }

        [Fact]
        public void FindUserByOrderShouldReturnNullIfInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindUserByOrderShouldReturnNullIfInvalidUser_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);
            this.SeedDbWithCountries(dbContext);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));
            var order = ordersService.FindUserOrderById("totallyLegitUserId", user.UserName);

            Assert.Null(order);
        }

        [Fact]
        public void FindUserByOrderShouldReturnNullIfInvalidOrderId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindUserByOrderShouldReturnNullIfInvalidOrderId_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);
            this.SeedDbWithCountries(dbContext);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));
            var order = ordersService.FindUserOrderById(orderId, "totallyLegitUserUsername");

            Assert.Null(order);
        }

        [Fact]
        public void OrderProductsByIdShouldReturnOrderProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"OrderProductsByIdShouldReturnOrderProducts_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);
            this.SeedDbWithCountries(dbContext);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));
            var orderProducts = ordersService.OrderProductsByOrderId(orderId);

            Assert.True(orderProducts.Count == 1 && orderProducts[0].Quantity == 2);
        }

        [Fact]
        public void FindAllCountriesShouldReturnListWithAllCountryNames()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllCountriesShouldReturnListWithAllCountryNames_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);

            this.SeedDbWithCountries(dbContext);

            Assert.True(dbContext.Countries.Count() == ordersService.FindAllCountries().Count);
        }

        [Fact]
        public void FindAllUserOrdersShouldReturnAllUserOrders()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllUserOrdersShouldReturnAllUserOrders_Orders_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);
            var productsService = new ProductsService(dbContext, mapper);
            var shoppingCartsService = new ShoppingCartsService(dbContext, productsService, usersService, mapper);
            var ordersService = new OrdersService(dbContext, shoppingCartsService, mapper);
            var favouritesService = new FavouritesService(dbContext, productsService, usersService, mapper);

            this.SeeDbdWithBrands(dbContext);
            this.SeedDbWithCategories(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var image = new Mock<IFormFile>();

            this.SeedDbWithCountries(dbContext);
            this.SeedDbWithUserAndProduct(dbContext, productsService, mapper.Map<Category>(categories[0]), mapper.Map<Brand>(brands[0]), image.Object);

            var user = dbContext.Users.FirstOrDefault(x => x.UserName == "1");
            var products = productsService.FindAllProducts();

            var shoppingCarts = dbContext.ShoppingCarts;
            var shoppingCartss = dbContext.ShoppingCartProducts;

            var cart = this.SeedDbShoppingCartWithProducts(dbContext, user.UserName, products[0].Id);

            var model = this.CreateOrderCreateBindingModel();
            var orderId = ordersService.CreateOrder(model, mapper.Map<ApplicationUserDTO>(user));
            var orders = ordersService.FindAllUserOrders(mapper.Map<ApplicationUserDTO>(user));

            Assert.True(orders.Count == 1);
        }

        private void SeedDbWithUserAndProduct(ApplicationDbContext dbContext, ProductsService productsService, Category category, Brand brand, IFormFile formfile)
        {
            for (int i = 0; i < 5; i++)
            {
                productsService.Create(new CreateProductBindingModel { Brand = brand.Name, Category = category.Name, Name = Guid.NewGuid().ToString(), Price = "15" }, formfile);
                dbContext.Users.Add(new ApplicationUser { UserName = $"{i}", FavouriteList = new FavouriteList(), ShoppingCart = new ShoppingCart { ShoppingCartProducts = new List<ShoppingCartProduct>() } });
                dbContext.SaveChanges();
            }
        }

        private ShoppingCart SeedDbShoppingCartWithProducts(ApplicationDbContext dbContext, string userName, string productId)
        {
            var shoppingCart = dbContext.ShoppingCarts
                .FirstOrDefault(x => x.Id == dbContext.Users
                .FirstOrDefault(y => y.UserName == userName).Id);
            var shoppingcartProducts = new List<ShoppingCartProduct>();
            var shoppingCartProduct = new ShoppingCartProduct { ProductId = dbContext.Products.FirstOrDefault(x => x.Id == productId).Id, ShoppingCartId = shoppingCart.Id, Quantity = 2 };
            shoppingcartProducts.Add(shoppingCartProduct);

            dbContext.ShoppingCartProducts.AddRange(shoppingcartProducts);
            shoppingCart.ShoppingCartProducts = shoppingcartProducts;
            dbContext.ShoppingCarts.Update(shoppingCart);
            dbContext.SaveChanges();
            return shoppingCart;
        }

        private void SeedDbWithCountries(ApplicationDbContext dbContext)
        {
            var country = new Country { Name = "Bulgaria" };
            var country1 = new Country { Name = "asddsa" };
            var country2 = new Country { Name = "asdddddd" };

            dbContext.Countries.AddRange(new List<Country> { country, country1, country2});
            dbContext.SaveChanges();
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

        private OrderCreateBindingModel CreateOrderCreateBindingModel()
        {
            return new OrderCreateBindingModel { FirstName = "legitFirstName", LastName = "legitLastName", City = "legitCity", Agree = true, OrderDate = DateTime.UtcNow, AddressLine2 = "totallyLegitAddressLine2", AddressLine1 = "totallyLegitAddressLine1", Country = "Bulgaria", DeliveryDate = DateTime.UtcNow, DeliveryPrice = 0, OrderStatus = OrderStatus.Processed, PaymentType = PaymentType.Paypal, PhoneNumber = "12342123", Region = "totallyLegitRegion", TotalPrice = 0, ZIP = "totallyLegitZip" };
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
