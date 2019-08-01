namespace Palitra27.Web.Controllers.Contact
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Web.ViewModels.Contacts;

    public class ContactController : BaseController
    {
        private const string receivingEmail = "mutenroshiyo69@gmail.com";

        private readonly IEmailSender emailSender;

        public ContactController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public IActionResult Info()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Info(ContactBindingModel model)
        {
            this.emailSender.SendEmailAsync(receivingEmail, $"{model.Email}", $"<p>Subject:{model.Subject} <br />Message:{model.Message}</p>");

            return this.Redirect("/Home/Index");
        }
    }
}
