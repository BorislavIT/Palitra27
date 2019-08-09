namespace Palitra27.Web.ViewModels.Brands
{
    using System.ComponentModel.DataAnnotations;

    public class CreateBrandBindingModel
    {
        [Required]
        [RegularExpression(@"[A-Za-z]+", ErrorMessage = "The brand name should have only English letters.")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Name { get; set; }
    }
}
