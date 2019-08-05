namespace Palitra27.Web.Areas.Administration.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Categories;
    using Palitra27.Web.ViewModels.Errors;

    public class CategoriesController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A category with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Categories/Create";

        private readonly ICategoriesService categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View(new CreateCategoryBindingModel { });
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
                var creationErrorViewModel = new CreationErrorViewModel { ErrorMessage = CreationAlreadyExistsErrorMessage, HyperLink = HyperLinkForCreationError };
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
        }
    }
}
