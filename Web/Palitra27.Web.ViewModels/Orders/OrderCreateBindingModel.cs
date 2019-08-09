namespace Palitra27.Web.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Palitra27.Data.Models.Enums;

    public class OrderCreateBindingModel
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z]+", ErrorMessage = "The first name should have only letters.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z]+", ErrorMessage = "The last name should have only letters.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "The phone number is not a valid phone number.")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "The phone number is not a valid phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9.\s]+", ErrorMessage = "The adressline1 is not a valid address")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string AddressLine1 { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9.\s]+", ErrorMessage = "The adressline2 is not a valid address")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string AddressLine2 { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z]+", ErrorMessage = "The city name should have only letters.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string City { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z]+", ErrorMessage = "The region name should have only letters.")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string Region { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
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
