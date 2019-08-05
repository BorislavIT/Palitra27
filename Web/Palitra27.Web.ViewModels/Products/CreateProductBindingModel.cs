namespace Palitra27.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class CreateProductBindingModel
    {
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Brand { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Category { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The \"{0}\" must be between {1} and {2}.")]
        public decimal Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
