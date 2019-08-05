namespace Palitra27.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Services.Mapping;

    public class ProductInfoViewModel : IMapFrom<Product>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Category { get; set; }

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
