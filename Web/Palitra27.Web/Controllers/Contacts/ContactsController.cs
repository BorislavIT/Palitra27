namespace Palitra27.Web.Controllers.Contact
{
    using Microsoft.AspNetCore.Mvc;

    public class ContactsController : BaseController
    {
        public IActionResult Info()
        {
            return this.View();
        }
    }
}
