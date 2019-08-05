namespace Palitra27.Web.Areas.Administration.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Categories;

    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoryService;

        public CategoriesController(ICategoriesService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryBindingModel model)
        {
            var category = this.categoryService.CreateCategory(model);

            return this.Redirect("/Home/Index");
        }
    }
}
