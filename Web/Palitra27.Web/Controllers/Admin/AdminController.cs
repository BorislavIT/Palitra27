namespace Palitra27.Web.Controllers.Admins
{
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
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateProductBindingModel model)
        {
            this.productsService.Create(model);
            return this.Redirect("/");
        }
    }
}
