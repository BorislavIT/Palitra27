namespace Palitra27.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class ProductEditBindingModel
    {
        public string Id { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9-]+", ErrorMessage = "The product name is should have only English letters.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]

        public string Name { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 1, ErrorMessage = "The \"{0}\" is too big, no one would buy it....")]
        public string Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        [RegularExpression(@"[A-Za-z0-9-.\s]+", ErrorMessage = "The mini description should have only English letters.")]
        public string MiniDescription { get; set; }
    }
}
