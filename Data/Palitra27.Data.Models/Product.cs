namespace Palitra27.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Palitra27.Data.Common.Models;

    public class Product : BaseDeletableModel<string>
    {
        [Required]
        public string BrandId { get; set; }

        [Required]
        public virtual Brand Brand { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public string Name { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Image { get; set; }

        public virtual List<Review> Reviews { get; set; } = new List<Review>();

        public string MiniDescription { get; set; } = "This product has no Description";

        public string Description { get; set; } = "This product has no Description";

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal Depth { get; set; }

        public decimal Weight { get; set; }

        public virtual ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; }
    }
}
