namespace Palitra27.Web.ViewModels.Orders
{
    using System;

    public class DeliveredOrdersViewModels
    {
        public string Id { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentType { get; set; }
    }
}