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
        private const int DefaultQuantity = 1;

        private readonly IShoppingCartsService shoppingCartService;
        private readonly IProductsService productSevice;
        private readonly IMapper mapper;

        public ShoppingCartController(
            IShoppingCartsService shoppingCartService,
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
                var shoppingCartProducts = this.shoppingCartService.FindAllShoppingCartProducts(this.User.Identity.Name).ToList();
                if (shoppingCartProducts.Count() == 0)
                {
                    return this.RedirectToAction("Index", "Home");
                }

                var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

                return this.View(shoppingCartProductsViewModel);
            }

            var shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey);
            if (shoppingCartSession == null || shoppingCartSession.Count == 0)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View(shoppingCartSession);
        }

        public IActionResult Add(string id, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (quantity <= 0)
                {
                    this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name, 1);
                }
                else
                {
                    this.shoppingCartService.AddProductInShoppingCart(id, this.User.Identity.Name, quantity);
                }
            }
            else
            {
                List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey);
                if (shoppingCartSession == null)
                {
                    shoppingCartSession = new List<ShoppingCartProductsViewModel>();
                }

                if (!shoppingCartSession.Any(x => x.Id == id))
                {
                    var product = this.productSevice.FindProductById(id);

                    var shoppingCart = this.mapper.Map<ShoppingCartProductsViewModel>(product);
                    if (quantity <= 0)
                    {
                        shoppingCart.Quantity = DefaultQuantity;
                    }
                    else
                    {
                        shoppingCart.Quantity = quantity;
                    }

                    shoppingCart.TotalPrice = shoppingCart.Quantity * shoppingCart.Price;

                    shoppingCartSession.Add(shoppingCart);

                    SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey, shoppingCartSession);
                }
                else
                {
                    var product = shoppingCartSession.First(x => x.Id == id);

                    var shoppingCart = this.mapper.Map<ShoppingCartProductsViewModel>(product);

                    shoppingCart.Quantity += quantity;

                    shoppingCart.TotalPrice = shoppingCart.Quantity * shoppingCart.Price;

                    SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey, shoppingCartSession);
                }
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(string id)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.RemoveProductFromShoppingCart(id, this.User.Identity.Name);

                return this.RedirectToAction(nameof(this.Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id))
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                shoppingCartSession.Remove(product);

                SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(string id, int quantity)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                this.shoppingCartService.EditProductQuantityInShoppingCart(id, this.User.Identity.Name, quantity);

                return this.RedirectToAction(nameof(this.Index));
            }

            List<ShoppingCartProductsViewModel> shoppingCartSession = SessionHelper.GetObjectFromJson<List<ShoppingCartProductsViewModel>>(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey);
            if (shoppingCartSession == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            if (shoppingCartSession.Any(x => x.Id == id) && quantity > 0)
            {
                var product = shoppingCartSession.First(x => x.Id == id);
                product.Quantity = quantity;
                product.TotalPrice = quantity * product.Price;

                SessionHelper.SetObjectAsJson(this.HttpContext.Session, GlobalConstants.SessionShoppingCartKey, shoppingCartSession);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
