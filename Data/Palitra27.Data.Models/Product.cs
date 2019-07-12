namespace Palitra27.Data.Models
{
    using Palitra27.Data.Common.Models;

    public class Product : BaseDeletableModel<int>
    {
        public string Id { get; set; }

        public ProductBrand Brand { get; set; }

        public string Name { get; set; }

        public string CategoryId { get; set; }

        public Category Category { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}
