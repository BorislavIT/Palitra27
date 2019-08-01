namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using Palitra27.Data.Models;
    using Palitra27.Services.Mapping;

    public class CreateProductBindingModel
    {
        public string Brand { get; set; }

        public string ProductName { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public IFormFile Image { get; set; }
    }
}
