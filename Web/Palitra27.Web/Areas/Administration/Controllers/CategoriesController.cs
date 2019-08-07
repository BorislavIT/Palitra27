namespace Palitra27.Web.Areas.Administration.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Categories;

    public class CategoriesController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A category with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Categories/Create";

        private readonly ICategoriesService categoryService;
        private readonly IErrorService errorService;

        public CategoriesController(
            ICategoriesService categoryService,
            IErrorService errorService)
        {
            this.categoryService = categoryService;
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
    }
}
