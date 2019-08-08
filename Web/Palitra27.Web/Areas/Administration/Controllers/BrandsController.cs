namespace Palitra27.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Brands;

    public class BrandsController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A brand with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Brands/Create";

        private const string DeletionDoesntExistErrorMessage = "A brand with such name doesn't exist, ";
        private const string HyperLinkForDoesntExistError = "/Administration/Brands/Create";

        private readonly IBrandsService brandService;
        private readonly IErrorsService errorService;

        public BrandsController(
            IBrandsService brandService,
            IErrorsService errorService)
        {
            this.brandService = brandService;
            this.errorService = errorService;
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
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(CreationAlreadyExistsErrorMessage, HyperLinkForCreationError);
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
        }

        public IActionResult Delete()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Delete(CreateBrandBindingModel model)
        {
            var brand = this.brandService.RemoveBrand(model);

            if (brand == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(DeletionDoesntExistErrorMessage, HyperLinkForDoesntExistError);
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
        }
    }
}
