namespace Palitra27.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Brands;
    using Palitra27.Web.ViewModels.Errors;

    public class BrandsController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A brand with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Brands/Create";

        private readonly IBrandsService brandService;

        public BrandsController(IBrandsService brandService)
        {
            this.brandService = brandService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateBrandBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var brand = this.brandService.CreateBrand(model);

            if (brand == null)
            {
                var creationErrorViewModel = new CreationErrorViewModel { ErrorMessage = CreationAlreadyExistsErrorMessage, HyperLink = HyperLinkForCreationError };
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
        }
    }
}
