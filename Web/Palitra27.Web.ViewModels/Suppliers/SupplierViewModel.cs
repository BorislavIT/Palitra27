using System;
using System.Collections.Generic;
using System.Text;

namespace Palitra27.Web.ViewModels.Suppliers
{
    public class SupplierViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal PriceToHome { get; set; }

        public decimal PriceToOffice { get; set; }

        public bool IsDefault { get; set; }
    }
}