namespace Palitra27.Web.ViewModels.Shop
{
    public class ShopViewModel
    {
        public string Category { get; set; }

        public string Brand { get; set; }

        public decimal PriceLower { get; set; }

        public decimal PriceUpper { get; set; }

        public string Sorting { get; set; }

        public int Show { get; set; }

        public int Page { get; set; }
    }
}
