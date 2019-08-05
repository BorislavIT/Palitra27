using System;
using System.Collections.Generic;
using System.Text;

namespace Palitra27.Web.ViewModels.Orders
{
    public class MyOrderViewModel
    {
        public string Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }
    }
}