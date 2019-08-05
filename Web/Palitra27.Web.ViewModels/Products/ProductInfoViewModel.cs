namespace Palitra27.Web.ViewModels.Products
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Services.Mapping;

    public class ProductInfoViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Name { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "The \"{0}\" must be between {1} and {2}.")]
        public decimal Price { get; set; }

        public string Image { get; set; }

        public List<ReviewDTO> Reviews { get; set; }

        public decimal Weight { get; set; }

        public decimal Height { get; set; }

        public decimal Depth { get; set; }

        public decimal Width { get; set; }

        public string Description { get; set; }

        public string MiniDescription { get; set; }
    }
}
