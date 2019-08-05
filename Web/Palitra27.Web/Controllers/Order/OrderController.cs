namespace Palitra27.Web.Controllers.Order
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Orders;
    using Palitra27.Web.ViewModels.ShoppingCart;

    public class OrderController : BaseController
    {
        private readonly IUserService usersService;
        private readonly IOrderService orderService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IMapper mapper;

        public OrderController(
            IUserService usersService,
            IOrderService orderService,
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
            if (this.User.Identity.IsAuthenticated)
            {
                var user = this.usersService.GetUserByUsername(this.User.Identity.Name);

                var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(user.Username);

                var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new ShoppingCartProductsViewModel
                {
                    Id = x.ProductId,
                    Image = x.Product.Image,
                    Name = x.Product.Name,
                    Price = x.Product.Price,
                    Quantity = x.Quantity,
                    TotalPrice = x.Quantity * x.Product.Price,
                }).ToList();

                var fillFormViewModel = new OrderCreateViewModel() { FirstName = user.FirstName, LastName = user.LastName, PhoneNumber = user.PhoneNumber };

                var countries = this.orderService.GetAllCountries();

                var actualModel = new OrderShoppingCartViewModel { ShoppingCartProductsViewModels = shoppingCartProductsViewModel, Countries = countries, OrderCreateViewModel = fillFormViewModel };

                return this.View(actualModel);
            }
            else
            {
                 return this.Redirect("af/agfdga");
            }
        }

        [HttpPost]
        public IActionResult Create(OrderCreateViewModel model)
        {
            var user = this.usersService.GetUserByUsername(this.User.Identity.Name);
            var orderId = this.orderService.CreateOrder(model, user);
            this.shoppingCartService.DeleteAllProductFromShoppingCart(this.User.Identity.Name);

            return this.Redirect($"/Order/Details/{orderId}");
        }

        public IActionResult Details(string id)
        {
            var user = this.usersService.GetDomainUserByUsername(this.User.Identity.Name);

            var order = this.orderService.GetUserOrderById(id, this.User.Identity.Name);

            var shoppingCartProducts = this.orderService.OrderProductsByOrderId(order.Id);

            var shoppingCartProductsViewModel = shoppingCartProducts.Select(x => new ShoppingCartProductsViewModel
            {
                Id = x.ProductId,
                Image = x.Product.Image,
                Name = x.Product.Name,
                Price = x.Product.Price,
                Quantity = x.Quantity,
                TotalPrice = x.Quantity * x.Product.Price,
            }).ToList();

            var fillFormViewModel = this.mapper.Map<OrderCreateViewModel>(order);

            var actualModel = new OrderShoppingCartViewModel { Id = order.Id, ShoppingCartProductsViewModels = shoppingCartProductsViewModel, OrderCreateViewModel = fillFormViewModel };

            return this.View(actualModel);
        }
    }
}