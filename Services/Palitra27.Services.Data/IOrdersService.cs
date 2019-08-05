namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        List<string> GetAllCountries();

        string CreateOrder(OrderCreateBindingModel model, ApplicationUserDTO user);

        OrderDTO GetUserOrderById(string id, string username);

        IEnumerable<OrderProduct> OrderProductsByOrderId(string id);
    }
}