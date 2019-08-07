namespace Palitra27.Web.ViewModels.Products
{
    using System.ComponentModel.DataAnnotations;

    public class EditSpecificationsBindingModel
    {
        public string Id { get; set; }

        [Range(0, 1000, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public decimal Width { get; set; }

        [Range(0, 1000, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public decimal Height { get; set; }

        [Range(0, 1000, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public decimal Depth { get; set; }

        [Range(0, 1000, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1} letters.")]
        public decimal Weight { get; set; }
    }
}
