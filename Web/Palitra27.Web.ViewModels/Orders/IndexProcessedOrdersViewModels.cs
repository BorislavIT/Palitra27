namespace Palitra27.Web.ViewModels.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class IndexProcessedOrdersViewModels
    {
        public string Id { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? DispatchDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentType { get; set; }
    }
}