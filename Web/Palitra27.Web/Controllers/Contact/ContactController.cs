namespace Palitra27.Web.Controllers.Contact
{
    using Microsoft.AspNetCore.Mvc;

    public class ContactController : BaseController
    {
        public IActionResult Info()
        {
            return this.View();
        }
    }
}
