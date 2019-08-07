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
        [StringLength(15, MinimumLength = 1, ErrorMessage = "The \"{0}\" is too big, no one would buy it....")]
        public string Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string MiniDescription { get; set; }
    }
}
