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

    public class ShoppingCartService : IShoppingCartService
    {
        private const int DefaultProductQuantity = 1;

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

                this.db.SaveChanges();
                return;
            }

            shoppingCartProduct = this.CreateShoppingCartProductByProduct(product, quantity, userCart);

            this.db.ShoppingCartProducts.Add(shoppingCartProduct);
            this.db.SaveChanges();
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

            this.db.ShoppingCartProducts.Remove(shoppingCart);
            this.db.SaveChanges();
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

            this.db.ShoppingCartProducts.RemoveRange(shoppingCartProducts);
            this.db.SaveChanges();
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

            this.db.Update(shoppingCartProduct);
            this.db.SaveChanges();
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
            return this.db.ShoppingCartProducts
                .FirstOrDefault(x => x.ShoppingCartId == shoppingCartId && x.ProductId == productId);
        }

        private ShoppingCart FindShoppingCartByUserId(ApplicationUserDTO user)
        {
            var userCart = this.db.ShoppingCarts
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
            var shoppingCartProduct = this.db.ShoppingCartProducts
               .FirstOrDefault(x => x.ProductId == product.Id);

            return shoppingCartProduct;
        }

        private List<ShoppingCartProduct> FindShoppingCartProductsByUser(ApplicationUserDTO user)
        {
            var shoppingCartProducts = this.db.ShoppingCartProducts
               .Where(x => x.ShoppingCartId == user.ShoppingCartId)
               .ToList();

            return shoppingCartProducts;
        }

        private List<ShoppingCartProduct> FindShoppingCartProductsByUserName(ApplicationUserDTO user)
        {
            var shoppingCartProducts = this.db.ShoppingCartProducts.Include(x => x.Product)
                                               .Include(x => x.ShoppingCart)
                                               .Where(x => x.ShoppingCart.User.UserName == user.Username)
                                               .Where(x => x.Product.Brand.IsDeleted == false && x.Product.Category.IsDeleted == false)
                                               .ToList();

            return shoppingCartProducts;
        }

        private bool CheckIfAnyProductsInShoppingCartByUsername(string username)
        {
            return this.db.ShoppingCartProducts
                .Any(x => x.ShoppingCart.User.UserName == username);
        }
    }
}
