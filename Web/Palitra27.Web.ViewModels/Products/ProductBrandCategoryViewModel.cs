namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Palitra27.Data.Models;

    public class ProductBrandCategoryViewModel
    {
        public List<Category> Categories { get; set; }

        public List<ProductBrand> Brands { get; set; }
    }
}
