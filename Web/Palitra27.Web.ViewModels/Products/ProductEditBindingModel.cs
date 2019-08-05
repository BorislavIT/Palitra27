namespace Palitra27.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class ProductEditBindingModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]

        public string Name { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The \"{0}\" must be between {2} and {1}.")]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string MiniDescription { get; set; }
    }
}
