namespace Palitra27.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Web.ViewModels.Errors;

    public class ErrorController : BaseController
    {
        public IActionResult NotFound()
        {
            return this.View();
        }

        public IActionResult CreationError(ErrorViewModel model)
        {
            return this.View(model);
        }
    }
}
