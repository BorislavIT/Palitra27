namespace Palitra27.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Palitra27.Data.Models.Enums;

    public class Order
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        public DateTime? OrderDate { get; set; }

        [Required]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        [Required]
        public decimal DeliveryPrice { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "The field \"{0}\" must have at least {2} letters and at most {1}.")]
        public string ZIP { get; set; }

        [Required]
        public string CountryId { get; set; }

        [Required]
        public virtual Country Country { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string LastName { get; set; }

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
        [StringLength(12, MinimumLength = 7, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} digits.")]
        public string PhoneNumber { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
