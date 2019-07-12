namespace Palitra27.Web.Controllers.Admins
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class AdminController : BaseController
    {
        private readonly IProductsService productsService;

        public AdminController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            return this.View(productCategoryBrandViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProductBindingModel model)
        {
            this.productsService.Create(model);
            return this.Redirect("/");
        }
    }
}
