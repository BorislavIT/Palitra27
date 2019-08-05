namespace Palitra27.Web.ViewModels.Orders
{
    using System;
    using Palitra27.Data.Models.Enums;

    public class OrderCreateBindingModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public int ZIP { get; set; }

        public string Country { get; set; }

        public bool Agree { get; set; }

        public PaymentType PaymentType { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
