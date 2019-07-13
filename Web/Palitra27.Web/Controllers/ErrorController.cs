namespace Palitra27.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : BaseController
    {
#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        public IActionResult NotFound()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            return this.View();
        }
    }
}
