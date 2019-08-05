namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EditSpecificationsBindingModel
    {
        public string Id { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }

        public decimal Depth { get; set; }

        public decimal Weight { get; set; }
    }
}
