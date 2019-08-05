namespace Palitra27.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Brands;

    public class BrandsController : AdministrationController
    {
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
            var brand = this.brandService.CreateBrand(model);
            return this.Redirect("/Home/Index");
        }
    }
}
