namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public class ShoppingCartService : IShoppingCartService
    {
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly ApplicationDbContext db;
        private readonly IProductsService productService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public ShoppingCartService(
            ApplicationDbContext db,
            IProductsService productService,
            IUserService userService,
            IMapper mapper)
        {
            this.db = db;
            this.productService = productService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public void AddProductInShoppingCart(string productId, string username, int? quntity = null)
        {
            var product = this.productService.FindDomainProduct(productId);
            var user = this.userService.GetUserByUsername(username);
            var userCart = this.db.ShoppingCarts.FirstOrDefault(x => x.User.Id == user.Id);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCartProduct = this.GetShoppingCartProduct(productId, userCart.Id);

            if (shoppingCartProduct != null)
            {
                if (quntity == null)
                {
                    shoppingCartProduct.Quantity++;
                }
                else
                {
                    shoppingCartProduct.Quantity += (int) quntity;
                }

                this.db.SaveChanges();
                return;
            }

            shoppingCartProduct = new ShoppingCartProduct
            {
                Product = product,
                Quantity = quntity == null ? DEFAULT_PRODUCT_QUANTITY : quntity.Value,
                ShoppingCartId = userCart.Id,
                ProductId = product.Id,
            };

            this.db.ShoppingCartProducts.Add(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public void DeleteProductFromShoppingCart(string id, string username)
        {
            var product = this.productService.GetOnlyProductById(id);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null)
            {
                return;
            }

            var shoppingCart = this.GetShoppingCartProduct(product.Id, user.ShoppingCartId);

            this.db.ShoppingCartProducts.Remove(shoppingCart);
            this.db.SaveChanges();
        }

        public bool AnyProducts(string username)
        {
            return this.db.ShoppingCartProducts.Any(x => x.ShoppingCart.User.UserName == username);
        }

        public void DeleteAllProductFromShoppingCart(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return;
            }

            var shoppingCartProducts = this.db.ShoppingCartProducts.Where(x => x.ShoppingCartId == user.ShoppingCartId).ToList();

            this.db.ShoppingCartProducts.RemoveRange(shoppingCartProducts);
            this.db.SaveChanges();
        }

        public void EditProductQuantityInShoppingCart(string productId, string username, int quantity)
        {
            var product = this.productService.GetOnlyProductById(productId);
            var user = this.userService.GetUserByUsername(username);

            if (product == null || user == null || quantity <= 0)
            {
                return;
            }

            var shoppingCartProduct = this.db.ShoppingCartProducts.FirstOrDefault(x => x.ProductId == productId);
            if (shoppingCartProduct == null)
            {
                return;
            }

            shoppingCartProduct.Quantity = quantity;

            this.db.Update(shoppingCartProduct);
            this.db.SaveChanges();
        }

        public IEnumerable<ShoppingCartProductDTO> GetAllShoppingCartProducts(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            var shoppingCartProducts = this.db.ShoppingCartProducts.Include(x => x.Product)
                                               .Include(x => x.ShoppingCart)
                                               .Where(x => x.ShoppingCart.User.UserName == username).ToList();

            return this.mapper.Map<List<ShoppingCartProductDTO>>(shoppingCartProducts);
        }

        public IEnumerable<ShoppingCartProduct> GetAllDomainShoppingCartProducts(string username)
        {
            var user = this.userService.GetUserByUsername(username);

            if (user == null)
            {
                return null;
            }

            return this.db.ShoppingCartProducts.Include(x => x.Product)
                                               .Include(x => x.ShoppingCart)
                                               .Where(x => x.ShoppingCart.User.UserName == username).ToList();
        }

        private ShoppingCartProduct GetShoppingCartProduct(string productId, string shoppingCartId)
        {
            return this.db.ShoppingCartProducts.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
        }
    }
}