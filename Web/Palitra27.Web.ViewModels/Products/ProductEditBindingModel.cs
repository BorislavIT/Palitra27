namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ProductEditBindingModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string MiniDescription { get; set; }
    }
}
