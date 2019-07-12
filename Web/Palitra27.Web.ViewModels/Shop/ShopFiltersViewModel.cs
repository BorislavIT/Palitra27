namespace Palitra27.Web.ViewModels.Shop
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Palitra27.Web.ViewModels.Products;

    public class ShopFiltersViewModel
    {
        public ProductListViewModel Products { get; set; }

        public ProductBrandCategoryViewModel ProductBrandCategoryViewModel { get; set; }
    }
}
