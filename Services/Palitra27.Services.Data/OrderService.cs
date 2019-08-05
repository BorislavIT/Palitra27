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
    using Palitra27.Data.Models.Enums;
    using Palitra27.Web.ViewModels.Orders;

    public class OrderService : IOrderService
    {
        private readonly IUserService userService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public OrderService(
            IUserService userService,
            IShoppingCartService shoppingCartService,
            ApplicationDbContext db,
            IMapper mapper)
        {
            this.userService = userService;
            this.shoppingCartService = shoppingCartService;
            this.db = db;
            this.mapper = mapper;
        }

        public string CreateOrder(OrderCreateViewModel model, ApplicationUserDTO user)
        {
            List<OrderProduct> orderProducts = new List<OrderProduct>();
            var shoppingCartProducts = this.shoppingCartService.GetAllDomainShoppingCartProducts(user.UserName).ToList();
            var country = this.FindCountryByName(model.Country);
            var order = new Order
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = int.Parse(model.PhoneNumber),
                PaymentStatus = PaymentStatus.Unpaid,
                PaymentType = model.PaymentType,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Processed,
                City = model.City,
                Country = country,
                CountryId = country.Id,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                Region = model.Region,
                ZIP = model.ZIP,
                UserId = user.Id,
                DeliveryPrice = 0,
                DeliveryDate = DateTime.UtcNow.AddDays(7),
            };
            foreach (var shoppingCartProduct in shoppingCartProducts)
            {
                var orderProduct = new OrderProduct
                {
                    Order = order,
                    Product = shoppingCartProduct.Product,
                    Quantity = shoppingCartProduct.Quantity,
                    Price = shoppingCartProduct.Product.Price,
                };

                orderProducts.Add(orderProduct);
            }

            order.OrderProducts = orderProducts;

            order.TotalPrice = order.OrderProducts.Sum(x => x.Quantity * x.Price) + (order.OrderProducts.Sum(x => x.Quantity * x.Price) * 0.2M);
            this.db.OrderProducts.AddRange(orderProducts);
            this.db.Orders.Add(order);
            this.db.SaveChanges();

            return order.Id;
        }

        public OrderDTO GetUserOrderById(string orderId, string username)
        {
            var order = this.db.Orders
                              .Include(x => x.User)
                              .Include(x => x.Country)
                              .FirstOrDefault(x => x.Id == orderId && x.User.UserName == username);
            return this.mapper.Map<OrderDTO>(order);
        }

        public IEnumerable<OrderProduct> OrderProductsByOrderId(string id)
        {
            return this.db.OrderProducts.Include(x => x.Product)
                                        .Where(x => x.OrderId == id).ToList();
        }

        private Country FindCountryByName(string name)
        {
            return this.db.Countries.FirstOrDefault(x => x.Name == name);
        }

        private OrderDTO MapOrderToOrderDTO(Order order)
        {
            var orderDTO = new OrderDTO
            {
                Country = order.Country,
                AddressLine1 = order.AddressLine1,
                AddressLine2 = order.AddressLine2,
                City = order.City,
                CountryId = order.CountryId,
                DeliveryDate = order.DeliveryDate,
                DeliveryPrice = order.DeliveryPrice,
                FirstName = order.FirstName,
                DiscountCoupons = order.DiscountCoupons,
                Id = order.Id,
                LastName = order.LastName,
                Notes = order.Notes,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                OrderDate = order.OrderDate,
                OrderProducts = order.OrderProducts,
                PaymentType = order.PaymentType,
                PhoneNumber = order.PhoneNumber,
                Region = order.Region,
                TotalPrice = order.TotalPrice,
                User = order.User,
                UserId = order.UserId,
                ZIP = order.ZIP,
            };
            return orderDTO;
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

        public List<string> GetAllCountries()
        {
            return this.db.Countries.Select(x => x.Name).ToList();
        }
    }
}