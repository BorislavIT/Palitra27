namespace Palitra27.Web.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AddReviewBindingModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }
    }
}
