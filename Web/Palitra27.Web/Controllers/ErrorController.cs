namespace Palitra27.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
        public IActionResult NotFound()
        {
            return this.View();
        }
    }
}
