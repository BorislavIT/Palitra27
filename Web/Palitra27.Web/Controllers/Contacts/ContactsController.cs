namespace Palitra27.Web.Controllers.Contact
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Web.ViewModels.Contacts;

    public class ContactsController : BaseController
    {
        private const string ReceivingEmail = "mutenroshiyo69@gmail.com";

        private readonly IEmailSender emailSender;

        public ContactsController(
            IEmailSender emailSender)
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
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.emailSender.SendEmailAsync(ReceivingEmail, $"{model.Email}", $"<p>Subject:{model.Subject} <br />Message:{model.Message}</p>");

            return this.Redirect("/Home/Index");
        }
    }
}
