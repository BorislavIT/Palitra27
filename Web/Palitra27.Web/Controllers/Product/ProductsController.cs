namespace Palitra27.Web.Controllers.Product
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public ProductsController(
            IProductsService productsService,
            IMapper mapper)
        {
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public IActionResult Info(string id)
        {
            var product = this.productsService.FindProductById(id);

            var productModel = this.mapper.Map<ProductInfoViewModel>(product);

            return this.View(productModel);
        }

        [HttpPost]
        [Authorize(Roles = GlobalConstants.UserRoleName + "," + GlobalConstants.AdministratorRoleName)]
        public IActionResult Info(string id, int qty)
        {
            return this.Redirect($"/ShoppingCart/Add/{id}?qty={qty}");
        }
    }
}
