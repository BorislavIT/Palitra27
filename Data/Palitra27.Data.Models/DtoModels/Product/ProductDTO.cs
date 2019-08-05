namespace Palitra27.Data.Models.DtoModels.Product
{
    using System.Collections.Generic;

    using Palitra27.Data.Common.Models;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;

    public class ProductDTO : BaseDeletableModel<string>
    {
        public string BrandId { get; set; }

        public virtual BrandDTO Brand { get; set; }

        public string Name { get; set; }

        public string CategoryId { get; set; }

        public virtual CategoryDTO Category { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }

        public virtual List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();

        public string MiniDescription { get; set; } = "This product has no Description";

        public string Description { get; set; } = "This product has no Description";

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal Depth { get; set; }

        public decimal Weight { get; set; }

        public virtual ICollection<ShoppingCartProductDTO> ShoppingCartProducts { get; set; }
    }
}
