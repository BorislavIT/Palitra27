namespace Palitra27.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Palitra27.Data.Models.Enums;
    using Palitra27.Web.ViewModels.Suppliers;

    public class CreateOrderViewModel
    {
        public IList<OrderAdressViewModel> OrderAddressesViewModel { get; set; }

        public OrderAdressViewModel OrderAdressViewModel { get; set; }

        public IList<SupplierViewModel> SuppliersViewModel { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public string SupplierId { get; set; }

        public string DeliveryAddressId { get; set; }

        public string FullName { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}