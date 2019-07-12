namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Palitra27.Data.Models;
    using Palitra27.Services.Mapping;

    public class CreateProductBindingModel : IMapTo<Product>
    {
        public string Brand { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public string Image { get; set; }
    }
}
