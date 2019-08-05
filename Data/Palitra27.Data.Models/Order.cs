namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Palitra27.Data.Models.Enums;

    public class Order
    {
        public string Id { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public int ZIP { get; set; }

        public string CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string Notes { get; set; }

        public int PhoneNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public virtual ICollection<DiscountCoupon> DiscountCoupons { get; set; }
    }
}
