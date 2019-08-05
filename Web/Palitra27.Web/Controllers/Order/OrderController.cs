namespace Palitra27.Web.Controllers.Order
{
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Orders;
    using Palitra27.Web.ViewModels.ShoppingCart;

    [Authorize(Roles = GlobalConstants.UserRoleName + "," + GlobalConstants.AdministratorRoleName)]
    public class OrderController : BaseController
    {
        private readonly IUserService usersService;
        private readonly IOrdersService orderService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;

        public OrderController(
            IUserService usersService,
            IOrdersService orderService,
            IShoppingCartService shoppingCartService,
            IMapper mapper)
        {
            this.usersService = usersService;
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
                var user = this.usersService.GetUserByUsername(this.User.Identity.Name);

                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(user.Username);

                var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

                var fillFormViewModel = new OrderCreateBindingModel() { FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber };

                var countries = this.orderService.GetAllCountries();

                var actualModel = new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, Countries = countries, OrderCreateViewModel = fillFormViewModel };

                return this.View(actualModel);
        }

        [HttpPost]
        public IActionResult Create(OrderCreateBindingModel model)
        {
            var user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            var countries = this.orderService.GetAllCountries();

            if (this.ModelState.IsValid)
            {
                var orderId = this.orderService.CreateOrder(model, user);
                if (orderId == string.Empty)
                {
                    var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(user.Username);

                    var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

                    var actualModel = new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, Countries = countries, OrderCreateViewModel = model };

                    return this.View(actualModel);
                }

                this.shoppingCartService.DeleteAllProductFromShoppingCart(this.User.Identity.Name);

                return this.Redirect($"/Order/Details/{orderId}");
            }
            else
            {
                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(user.Username);
                var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);
                var actualModel = new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, Countries = countries, OrderCreateViewModel = model };

                return this.View(actualModel);
            }
        }

        public IActionResult Details(string id)
        {
            var order = this.orderService.GetUserOrderById(id, this.User.Identity.Name);

            var shoppingCartProducts = this.orderService.OrderProductsByOrderId(order.Id);

            var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

            var fillFormViewModel = this.mapper.Map<OrderCreateBindingModel>(order);

            var actualModel = new OrderShoppingCartViewModel { Id = order.Id, ShoppingCartProductsViewModels = shoppingCartProductsViewModel, OrderCreateViewModel = fillFormViewModel };

            return this.View(actualModel);
        }
    }
}