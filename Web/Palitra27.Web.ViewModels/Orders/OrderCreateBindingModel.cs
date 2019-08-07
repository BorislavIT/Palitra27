namespace Palitra27.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Palitra27.Data.Models.Enums;

    public class OrderCreateBindingModel
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string AddressLine1 { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string City { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string Region { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string ZIP { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public bool Agree { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Please pick a payment method!")]

        public PaymentType PaymentType { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
