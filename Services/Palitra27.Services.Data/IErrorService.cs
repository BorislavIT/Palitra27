namespace Palitra27.Services.Data
{

    using Palitra27.Web.ViewModels.Errors;

    public interface IErrorService
    {
        ErrorViewModel CreateCreateionErrorViewModel(string errorMessage, string hyperLink);
    }
}
