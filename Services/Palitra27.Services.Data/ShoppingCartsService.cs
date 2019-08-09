namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public class ShoppingCartsService : IShoppingCartsService
    {
        private const int DefaultProductQuantity = 1;

        private readonly ApplicationDbContext dbContext;
        private readonly IProductsService productService;
        private readonly IUsersService userService;
        private readonly IMapper mapper;

        public ShoppingCartsService(
            ApplicationDbContext dbContext,
            IProductsService productService,
            IUsersService userService,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.productService = productService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public void AddProductInShoppingCart(string productId, string username, int? quantity = null)
        {
            var product = this.productService.FindDomainProduct(productId);
            var user = this.userService.FindUserByUsername(username);
            var userCart = this.FindShoppingCartByUserId(user);

            if (this.CheckIfProductOrUserIsNull(product, user))
            {
                return;
            }

            var shoppingCartProduct = this.FindShoppingCartProduct(productId, userCart.Id);

            if (shoppingCartProduct != null)
            {
                if (quantity == null)
                {
                    shoppingCartProduct.Quantity++;
                }
                else
                {
                    shoppingCartProduct.Quantity += (int)quantity;
                }

                this.dbContext.SaveChanges();
                return;
            }

            shoppingCartProduct = this.CreateShoppingCartProductByProduct(product, quantity, userCart);

            this.dbContext.ShoppingCartProducts.Add(shoppingCartProduct);
            this.dbContext.SaveChanges();
        }

        public void DeleteProductFromShoppingCart(string id, string username)
        {
            var product = this.productService.FindDomainProduct(id);
            var user = this.userService.FindUserByUsername(username);

            if (this.CheckIfProductOrUserIsNull(product, user))
            {
                return;
            }

            var shoppingCart = this.FindShoppingCartProduct(product.Id, user.ShoppingCartId);

            this.dbContext.ShoppingCartProducts.Remove(shoppingCart);
            this.dbContext.SaveChanges();
        }

        public bool AnyProducts(string username)
        {
            var result = this.CheckIfAnyProductsInShoppingCartByUsername(username);

            return result;
        }

        public void DeleteAllProductFromShoppingCart(string username)
        {
            var user = this.userService.FindUserByUsername(username);

            if (user == null)
            {
                return;
            }

            var shoppingCartProducts = this.FindShoppingCartProductsByUser(user);

            this.dbContext.ShoppingCartProducts.RemoveRange(shoppingCartProducts);
            this.dbContext.SaveChanges();
        }

        public void EditProductQuantityInShoppingCart(string productId, string username, int quantity)
        {
            var product = this.productService.FindDomainProduct(productId);
            var user = this.userService.FindUserByUsername(username);

            if (this.CheckIfProductOrUserIsNull(product, user) || quantity <= 0)
            {
                return;
            }

            var shoppingCartProduct = this.FindShoppingCartProductByProductId(product);
            if (shoppingCartProduct == null)
            {
                return;
            }

            shoppingCartProduct.Quantity = quantity;

            this.dbContext.Update(shoppingCartProduct);
            this.dbContext.SaveChanges();
        }

        public List<ShoppingCartProductDTO> FindAllShoppingCartProducts(string username)
        {
            var user = this.userService.FindUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            var shoppingCartProducts = this.FindShoppingCartProductsByUserName(user);

            return this.mapper.Map<List<ShoppingCartProductDTO>>(shoppingCartProducts);
        }

        public List<ShoppingCartProduct> FindAllDomainShoppingCartProducts(string username)
        {
            var user = this.userService.FindUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            var shoppingCartProducts = this.FindShoppingCartProductsByUserName(user);

            return shoppingCartProducts;
        }

        private ShoppingCartProduct FindShoppingCartProduct(string productId, string shoppingCartId)
        {
            return this.dbContext.ShoppingCartProducts
                .FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
        }

        private ShoppingCart FindShoppingCartByUserId(ApplicationUserDTO user)
        {
            var userCart = this.dbContext.ShoppingCarts
                .FirstOrDefault(x => x.User.Id == user.Id);

            return userCart;
        }

        private bool CheckIfProductOrUserIsNull(Product product, ApplicationUserDTO user)
        {
            if (product == null || user == null)
            {
                return true;
            }

            return false;
        }

        private ShoppingCartProduct CreateShoppingCartProductByProduct(Product product, int? quantity, ShoppingCart shoppingCart)
        {
            var shoppingCartProduct = new ShoppingCartProduct
            {
                Product = product,
                Quantity = quantity == null ? DefaultProductQuantity : quantity.Value,
                ShoppingCartId = shoppingCart.Id,
                ProductId = product.Id,
            };

            return shoppingCartProduct;
        }

        private ShoppingCartProduct FindShoppingCartProductByProductId(Product product)
        {
            var shoppingCartProduct = this.dbContext.ShoppingCartProducts
               .FirstOrDefault(x => x.ProductId == product.Id);

            return shoppingCartProduct;
        }

        private List<ShoppingCartProduct> FindShoppingCartProductsByUser(ApplicationUserDTO user)
        {
            var shoppingCartProducts = this.dbContext.ShoppingCartProducts
               .Where(x => x.ShoppingCartId == user.ShoppingCartId)
               .ToList();

            return shoppingCartProducts;
        }

        private List<ShoppingCartProduct> FindShoppingCartProductsByUserName(ApplicationUserDTO user)
        {
            var shoppingCartProducts = this.dbContext.ShoppingCartProducts.Include(x => x.Product)
                                               .Include(x => x.ShoppingCart)
                                               .Where(x => x.ShoppingCart.User.UserName == user.Username)
                                               .Where(x => x.Product.Brand.IsDeleted == false && x.Product.Category.IsDeleted == false && x.Product.IsDeleted == false)
                                               .ToList();

            return shoppingCartProducts;
        }

        private bool CheckIfAnyProductsInShoppingCartByUsername(string username)
        {
            return this.dbContext.ShoppingCartProducts
                .Any(x => x.ShoppingCart.User.UserName == username);
        }
    }
}
