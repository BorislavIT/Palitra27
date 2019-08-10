namespace Palitra27.Web.Controllers.Order
{
    using System.Collections.Generic;

    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Orders;
    using Palitra27.Web.ViewModels.ShoppingCart;

    [Authorize(Roles = GlobalConstants.UserRoleName + "," + GlobalConstants.AdministratorRoleName)]
    public class OrdersController : BaseController
    {
        private const string NoProductsInShoppingCartErrorMessage = "Unfortunately your shopping cart is empty, fill it up and,  ";
        private const string HyperLinkForDoesntExistError = "/Shop/Index";

        private const string DoesntExistOrNotYoursErrorMessage = "The order you're looking for isn't yours or doesn't exist.";
        private const string HyperLinkForDoesntExistOrNotYoursError = "/Home/Index";

        private const string NoOrdersErrorMessage = "You have no orders, make some and then try again!";
        private const string HyperLinkForNoOrdersErorr = "/Shop/Index";

        private readonly IUsersService usersService;
        private readonly IOrdersService orderService;
        private readonly IShoppingCartsService shoppingCartService;
        private readonly IMapper mapper;
        private readonly IErrorsService errorService;

        public OrdersController(
            IUsersService usersService,
            IOrdersService orderService,
            IShoppingCartsService shoppingCartService,
            IMapper mapper,
            IErrorsService errorService)
        {
            this.usersService = usersService;
            this.orderService = orderService;
            this.shoppingCartService = shoppingCartService;
            this.mapper = mapper;
            this.errorService = errorService;
        }

        public IActionResult Create()
        {
            var user = this.usersService.FindUserByUsername(this.User.Identity.Name);

            var shoppingCartProducts = this.shoppingCartService.FindAllShoppingCartProducts(user.Username);

            if (shoppingCartProducts.Count == 0)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(NoProductsInShoppingCartErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

            var fillFormViewModel = this.CreateOrderCreateBindingModel(user);

            var countries = this.orderService.FindAllCountries();

            var actualModel = this.CreateOrderShoppingCartViewWithCountries(fillFormViewModel, countries, shoppingCartProductsViewModel);

            return this.View(actualModel);
        }

        [HttpPost]
        public IActionResult Create(OrderCreateBindingModel model)
        {
            var user = this.usersService.FindUserByUsername(this.User.Identity.Name);
            var countries = this.orderService.FindAllCountries();
            var shoppingCartProducts = this.shoppingCartService.FindAllShoppingCartProducts(user.Username);

            if (shoppingCartProducts == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(NoProductsInShoppingCartErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

            if (this.ModelState.IsValid)
            {
                var orderId = this.orderService.CreateOrder(model, user);
                if (orderId == string.Empty)
                {
                    var actualModel = this.CreateOrderShoppingCartViewWithCountries(model, countries, shoppingCartProductsViewModel);

                    return this.View(actualModel);
                }

                this.shoppingCartService.RemoveAllProductsFromShoppingCart(this.User.Identity.Name);

                return this.Redirect($"/Orders/Details/{orderId}");
            }
            else
            {
                var actualModel = this.CreateOrderShoppingCartViewWithCountries(model, countries, shoppingCartProductsViewModel);

                return this.View(actualModel);
            }
        }

        public IActionResult Details(string id)
        {
            var order = this.orderService.FindUserOrderById(id, this.User.Identity.Name);

            if (order == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(DoesntExistOrNotYoursErrorMessage, HyperLinkForDoesntExistOrNotYoursError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var shoppingCartProducts = this.orderService.OrderProductsByOrderId(order.Id);

            var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

            var fillFormViewModel = this.mapper.Map<OrderCreateBindingModel>(order);

            var actualModel = this.CreateOrderShoppingCartViewWithoutCountries(fillFormViewModel, shoppingCartProductsViewModel);

            return this.View(actualModel);
        }

        public IActionResult All()
        {
            var user = this.usersService.FindUserByUsername(this.User.Identity.Name);

            var orders = this.orderService.FindAllUserOrders(user);

            if (orders.Count == 0)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(NoOrdersErrorMessage, HyperLinkForNoOrdersErorr);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var actualModels = new List<OrderShoppingCartViewModel>();

            foreach (var order in orders)
            {
                var shoppingCartProducts = this.orderService.OrderProductsByOrderId(order.Id);

                var shoppingCartProductsViewModel = this.mapper.Map<List<ShoppingCartProductsViewModel>>(shoppingCartProducts);

                var fillFormViewModel = this.mapper.Map<OrderCreateBindingModel>(order);

                var actualModel = this.CreateOrderShoppingCartViewWithoutCountries(fillFormViewModel, shoppingCartProductsViewModel);

                actualModels.Add(actualModel);
            }

            return this.View(actualModels);
        }

        private OrderShoppingCartViewModel CreateOrderShoppingCartViewWithoutCountries(OrderCreateBindingModel fillFormViewModel, List<ShoppingCartProductsViewModel> shoppingCartProductsViewModel)
        {
            return new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, OrderCreateViewModel = fillFormViewModel, Id = fillFormViewModel.Id };
        }

        private OrderShoppingCartViewModel CreateOrderShoppingCartViewWithCountries(OrderCreateBindingModel model, List<string> countries, List<ShoppingCartProductsViewModel> shoppingCartProductsViewModel)
        {
            return new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, Countries = countries, OrderCreateViewModel = model };
        }

        private OrderCreateBindingModel CreateOrderCreateBindingModel(ApplicationUserDTO user)
        {
            return new OrderCreateBindingModel() { FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber };
        }
    }
}