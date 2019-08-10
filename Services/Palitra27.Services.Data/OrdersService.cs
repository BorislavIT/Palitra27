namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Web.ViewModels.Orders;

    public class OrdersService : IOrdersService
    {
        private readonly IShoppingCartsService shoppingCartService;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public OrdersService(
            ApplicationDbContext dbContext,
            IShoppingCartsService shoppingCartService,
            IMapper mapper)
        {
            this.shoppingCartService = shoppingCartService;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public string CreateOrder(OrderCreateBindingModel model, ApplicationUserDTO user)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();

            var shoppingCartProducts = this.shoppingCartService.FindAllDomainShoppingCartProducts(user.UserName).ToList();
            if (shoppingCartProducts.Count == 0)
            {
                return string.Empty;
            }

            var country = this.FindCountryByName(model.Country);
            var order = this.MapOrderAndSetNewId(model, user, country);

            orderProducts = this.CreateOrderProductsFromShoppingCartProducts(orderProducts, shoppingCartProducts, order);

            order.OrderProducts = orderProducts;
            order.TotalPrice = this.CalculateTotalPrice(order);

            this.dbContext.OrderProducts.AddRange(orderProducts);
            this.dbContext.Orders.Add(order);
            this.dbContext.SaveChanges();

            return order.Id;
        }

        public OrderDTO FindUserOrderById(string id, string username)
        {
            var order = this.FindUserOrderByIdAndUsername(id, username);

            if (order == null)
            {
                return null;
            }

            return this.mapper.Map<OrderDTO>(order);
        }

        public List<OrderProduct> OrderProductsByOrderId(string id)
        {
            return this.FindOrderProductsByOrderId(id);
        }

        public List<string> FindAllCountries()
        {
            return this.dbContext.Countries.Select(x => x.Name)
                .ToList();
        }

        public List<OrderDTO> FindAllUserOrders(ApplicationUserDTO user)
        {
            return this.mapper.Map<List<OrderDTO>>(
                this.dbContext.Orders
                .Include(x => x.Country)
                .Include(x => x.User)
                .Where(x => x.UserId == user.Id));
        }

        private List<OrderProduct> FindOrderProductsByOrderId(string id)
        {
            return this.dbContext.OrderProducts.Include(x => x.Product)
                                        .Where(x => x.OrderId == id).ToList();
        }

        private Country FindCountryByName(string name)
        {
            return this.dbContext.Countries.FirstOrDefault(x => x.Name == name);
        }

        private decimal CalculateTotalPrice(Order order)
        {
            var totalPrice = order.OrderProducts.Sum(x => x.Quantity * x.Price) + (order.OrderProducts.Sum(x => x.Quantity * x.Price) * 0.2M);

            return totalPrice;
        }

        private OrderProduct CreateOrderProduct(Order order, ShoppingCartProduct shoppingCartProduct)
        {
            var orderProduct = new OrderProduct
            {
                Order = order,
                Product = shoppingCartProduct.Product,
                Quantity = shoppingCartProduct.Quantity,
                Price = shoppingCartProduct.Product.Price,
            };

            return orderProduct;
        }

        private Order FindUserOrderByIdAndUsername(string id, string username)
        {
            var order = this.dbContext.Orders
                .Include(x => x.Country)
                .FirstOrDefault(x => x.Id == id && x.User.UserName == username);

            if (order == null)
            {
                return null;
            }

            return order;
        }

        private List<OrderProduct> CreateOrderProductsFromShoppingCartProducts(List<OrderProduct> orderProducts, List<ShoppingCartProduct> shoppingCartProducts, Order order)
        {
            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var orderProduct = this.CreateOrderProduct(order, shoppingCartProduct);

                orderProducts.Add(orderProduct);
            }

            return orderProducts;
        }

        private Order MapOrderAndSetNewId(OrderCreateBindingModel model, ApplicationUserDTO user, Country country)
        {
            var order = this.mapper.Map<Order>(model);
            this.mapper.Map<ApplicationUserDTO, Order>(user, order);
            this.mapper.Map<Country, Order>(country, order);

            order.Id = Guid.NewGuid().ToString();
            return order;
        }

        //public void CompleteProcessingOrder(string username)
        //{
        //    var order = this.GetProcessingOrder(username);
        //    if (order == null)
        //    {
        //        return;
        //    }

        //    var shoppingCartProducts = this.shoppingCartService.GetAllShoppingCartProducts(username).ToList();
        //    if (shoppingCartProducts == null || shoppingCartProducts.Count == 0)
        //    {
        //        return;
        //    }

        //    List<OrderProduct> orderProducts = new List<OrderProduct>();

        //    foreach (var shoppingCartProduct in shoppingCartProducts)
        //    {
        //        var orderProduct = new OrderProduct
        //        {
        //            Order = order,
        //            Product = shoppingCartProduct.Product,
        //            Quantity = shoppingCartProduct.Quantity,
        //            Price = shoppingCartProduct.Product.Price,
        //        };

        //        orderProducts.Add(orderProduct);
        //    }

        //    this.shoppingCartService.DeleteAllProductFromShoppingCart(username);

        //    order.OrderDate = DateTime.Now;
        //    order.Status = OrderStatus.Unprocessed;
        //    order.PaymentStatus = PaymentStatus.Unpaid;
        //    order.OrderProducts = orderProducts;
        //    order.TotalPrice = order.OrderProducts.Sum(x => x.Quantity * x.Price);

        //    this.db.SaveChanges();
        //}

        //public Order GetOrderById(string orderId)
        //{
        //    return this.db.Orders.Include(x => x.DeliveryAddress)
        //                         .ThenInclude(x => x.City)
        //                         .Include(x => x.User)
        //                         .FirstOrDefault(x => x.Id == orderId);
        //}

        //public IEnumerable<Order> GetUnprocessedOrders()
        //{
        //    var orders = this.db.Orders.Include(x => x.DeliveryAddress)
        //                               .ThenInclude(x => x.City)
        //                               .Include(x => x.OrderProducts)
        //                               .Where(x => x.Status == OrderStatus.Unprocessed);

        //    return orders;
        //}

        //public Order GetProcessingOrder(string username)
        //{
        //    var user = this.userService.GetUserByUsername(username);

        //    if (user == null)
        //    {
        //        return null;
        //    }

        //    var order = this.db.Orders.Include(x => x.DeliveryAddress)
        //                              .ThenInclude(x => x.City)
        //                              .Include(x => x.OrderProducts)
        //                              .FirstOrDefault(x => x.User.UserName == username && x.Status == OrderStatus.Processing);

        //    return order;
        //}

        //public IEnumerable<Order> GetProcessedOrders()
        //{
        //    var orders = this.db.Orders.Include(x => x.DeliveryAddress)
        //                               .ThenInclude(x => x.City)
        //                               .Include(x => x.OrderProducts)
        //                               .Where(x => x.Status == OrderStatus.Processed);

        //    return orders;
        //}

        //public IEnumerable<Order> GetUserOrders(string username)
        //{
        //    var order = this.db.Orders.Where(x => x.User.UserName == username && x.Status != OrderStatus.Processing).ToList();

        //    return order;
        //}

        //public void SetOrderDetails(Order order, string fullName, PaymentType paymentType, string deliveryAddressId, decimal deliveryPrice)
        //{
        //    order.Recipient = fullName;
        //    order.PaymentType = paymentType;
        //    order.DeliveryAddressId = deliveryAddressId;
        //    order.DeliveryPrice = deliveryPrice;

        //    this.db.Update(order);
        //    this.db.SaveChanges();
        //}

        //public void ProcessOrder(string id)
        //{
        //    var order = this.db.Orders.FirstOrDefault(x => x.Id == id &&
        //                                (x.Status == OrderStatus.Unprocessed || x.Status == OrderStatus.Delivered));

        //    if (order == null)
        //    {
        //        return;
        //    }

        //    order.Status = OrderStatus.Processed;
        //    order.DeliveryDate = DateTime.Now.AddDays(7);
        //    this.db.SaveChanges();
        //}

        //public void DeliverOrder(string id)
        //{
        //    var order = this.db.Orders.FirstOrDefault(x => x.Id == id
        //                                    && x.Status == OrderStatus.Processed);

        //    if (order == null)
        //    {
        //        return;
        //    }

        //    order.Status = OrderStatus.Delivered;
        //    order.DeliveryDate = DateTime.Now;
        //    this.db.SaveChanges();
        //}



        //public IEnumerable<Order> GetDeliveredOrders()
        //{
        //    var orders = this.db.Orders.Include(x => x.DeliveryAddress)
        //                              .ThenInclude(x => x.City)
        //                              .Include(x => x.OrderProducts)
        //                              .Where(x => x.Status == OrderStatus.Delivered);

        //    return orders;
        //}

        //public void SetEasyPayNumber(Order order, string easyPayNumber)
        //{
        //    order.EasyPayNumber = easyPayNumber;
        //    this.db.SaveChanges();
        //}

        //public bool SetOrderStatusByInvoice(string invoiceNumber, string status)
        //{
        //    var isOrderStatus = Enum.TryParse(typeof(PaymentStatus), status, true, out object paymentStatus);
        //    var order = this.db.Orders.FirstOrDefault(x => x.InvoiceNumber == invoiceNumber);

        //    if (order == null || !isOrderStatus)
        //    {
        //        return false;
        //    }

        //    order.PaymentStatus = (PaymentStatus)paymentStatus;
        //    this.db.SaveChanges();
        //    return true;
        //}
    }
}