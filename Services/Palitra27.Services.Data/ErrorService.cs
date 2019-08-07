﻿namespace Palitra27.Services.Data
{
    using Palitra27.Web.ViewModels.Errors;

    public class ErrorService : IErrorService
    {
        public ErrorViewModel CreateCreateionErrorViewModel(string errorMessage, string hyperLink)
        {
            var creationErrorViewModel = new ErrorViewModel { ErrorMessage = errorMessage, HyperLink = hyperLink };

            return creationErrorViewModel;
        }
    }
}
