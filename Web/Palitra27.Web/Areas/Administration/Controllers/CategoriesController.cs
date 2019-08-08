namespace Palitra27.Web.Areas.Administration.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Categories;

    public class CategoriesController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A category with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Categories/Create";

        private const string DeletionDoesntExistErrorMessage = "A category with such name doesn't exist, ";
        private const string HyperLinkForDoesntExistError = "/Administration/Categories/Create";

        private readonly ICategoriesService categoryService;
        private readonly IErrorsService errorService;

        public CategoriesController(
            ICategoriesService categoriesService,
            IErrorsService errorService)
        {
            this.categoryService = categoriesService;
            this.errorService = errorService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var category = this.categoryService.CreateCategory(model);

            if (category == null)
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
        public IActionResult Delete(CreateCategoryBindingModel model)
        {
            var brand = this.categoryService.RemoveCategory(model);

            if (brand == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(DeletionDoesntExistErrorMessage, HyperLinkForDoesntExistError);
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
        }
    }
}
