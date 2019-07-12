namespace Palitra27.Web.ViewModels.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ShopViewModel
    {
        public string Category { get; set; }

        public string Brand { get; set; }

        public decimal PriceLower { get; set; }

        public decimal PriceUpper { get; set; }
    }
}
