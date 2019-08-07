namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Web.ViewModels.Orders;

    public interface IOrdersService
    {
        List<string> FindAllCountries();

        string CreateOrder(OrderCreateBindingModel model, ApplicationUserDTO user);

        OrderDTO FindUserOrderById(string id, string username);

        List<OrderProduct> OrderProductsByOrderId(string id);

        List<OrderDTO> FindAllUserOrders(ApplicationUserDTO user);
    }
}