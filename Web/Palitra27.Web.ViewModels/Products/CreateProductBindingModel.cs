namespace Palitra27.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class CreateProductBindingModel
    {
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public string Brand { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "The product name is invalid.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public string Category { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "The \"{0}\" is too big, no one would buy it....")]
        public string Price { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
