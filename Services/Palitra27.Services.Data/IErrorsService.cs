namespace Palitra27.Services.Data
{
    using Palitra27.Web.ViewModels.Errors;

    public interface IErrorsService
    {
        ErrorViewModel CreateCreateionErrorViewModel(string errorMessage, string hyperLink);
    }
}
