namespace Palitra27.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.ShoppingCart;

    public class ShoppingCartController : BaseController
    {
        private const int DEFAULT_PRODUCT_QUANTITY = 1;

        private readonly IShoppingCartService shoppingCartService;
        private readonly IProductsService productSevice;
        private readonly IMapper mapper;

        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IProductsService productSevice,
            IMapper mapper)
        {
            this.shoppingCartService = shoppingCartService;
            this.productSevice = productSevice;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(this.User.Identity.Name);
                if (shoppingCartProducts.Count() == 0)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new ShoppingCartProductsViewModel
                {
                    Id = x.ProductId,
                    Image = x.Product.Image,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Quantity = x.Quantity,
                    TotalPrice = x.Quantity * x.Product.Price,
                }).ToList();

                return this.View(shoppingCartProductsViewModel);
            }

            var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null || shoppingCartSession.Count == 0)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(shoppingCartSession);
        }

        public IActionResult Add(string id, bool direct, int qty)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (qty == 0)
                {
                    this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name, 1);
                }
                else
                {
                    this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name, qty);
                }
            }
            else
            {
                List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingCartProductsViewModel>();
                }

                if (!shoppingCartSession.Any(x => x.Id == id))
                {
                    var product = this.productSevice.FindProductById(id);

                    var shoppingCart = this.mapper.Map<ShoppingCartProductsViewModel>(product);
                    shoppingCart.Quantity = DEFAULT_PRODUCT_QUANTITY;
                    shoppingCart.TotalPrice = shoppingCart.Quantity * shoppingCart.Price;

                    shoppingCartSession.Add(shoppingCart);

                    SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
                }
            }

            if (direct == true)
            {
                return this.RedirectToAction("Create", "Orders");
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(string id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.DeleteProductFromShoppingCart(id, this.User.Identity.Name);

                return this.RedirectToAction(nameof(this.Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id))
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                shoppingCartSession.Remove(product);

                SessionHelper.SetObjectAsJson(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.EditProductQuantityInShoppingCart(id, this.User.Identity.Name, quantity);

                return this.RedirectToAction(nameof(Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id) && quantity > 0)
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                product.Quantity = quantity;
                product.TotalPrice = quantity * product.Price;

                SessionHelper.SetObjectAsJson(HttpContext.Session, GlobalConstants.SESSION_SHOPPING_CART_KEY, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(Index));
        }
    }
}
