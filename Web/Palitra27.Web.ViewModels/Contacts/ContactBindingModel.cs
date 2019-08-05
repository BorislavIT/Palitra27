namespace Palitra27.Web.ViewModels.Contacts
{
    using System.ComponentModel.DataAnnotations;

    public class ContactBindingModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Subject { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "The field \"{0}\" must have at least {2} and at most {1}.")]
        public string Message { get; set; }
    }
}
