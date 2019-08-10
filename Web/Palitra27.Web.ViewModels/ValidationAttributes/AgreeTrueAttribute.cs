namespace Palitra27.Web.ViewModels.ValidationAttributes
{
    using System.ComponentModel.DataAnnotations;

    public class AgreeTrueAttribute : ValidationAttribute
    {
        public AgreeTrueAttribute()
       : base("You must {0} to our terms & conditions in order to complete your request.") { }

        public override bool IsValid(object value) =>
      value is bool valueAsBool && valueAsBool;
    }
}
