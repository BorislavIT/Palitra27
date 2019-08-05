namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Web.ViewModels.Orders;

    public interface IOrderService
    {
        List<string> GetAllCountries();

        string CreateOrder(OrderCreateViewModel model, ApplicationUserDTO user);

        OrderDTO GetUserOrderById(string orderId, string username);

        IEnumerable<OrderProduct> OrderProductsByOrderId(string id);
    }
}