using System.ComponentModel.DataAnnotations;

namespace Palitra27.Web.ViewModels.Products
{
    public class EditDescriptionBindingModel
    {
        public string Id { get; set; }

        [RegularExpression(@"[A-Za-z0-9-.\s]+", ErrorMessage = "The mini description should have only English letters.")]
        public string Description { get; set; }
    }
}
