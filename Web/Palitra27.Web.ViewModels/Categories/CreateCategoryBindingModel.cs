namespace Palitra27.Web.ViewModels.Categories

{
    using System.ComponentModel.DataAnnotations;

    public class CreateCategoryBindingModel
    {
        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Name { get; set; }
    }
}
