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
        private const string ProductDoesntExistErrorMessage = "That product doesn't exist, ";
        private const string HyperLinkForDoesntExistError = "/Shop/Index";

        private readonly IProductsService productsService;
        private readonly IMapper mapper;
        private readonly IErrorService errorService;

        public ProductsController(
            IProductsService productsService,
            IMapper mapper,
            IErrorService errorService)
        {
            this.productsService = productsService;
            this.mapper = mapper;
            this.errorService = errorService;
        }

        public IActionResult Info(string id)
        {
            var product = this.productsService.FindProductById(id);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var productModel = this.mapper.Map<ProductInfoViewModel>(product);

            return this.View(productModel);
        }

        [HttpPost]
        public IActionResult Info(string id, int quantity)
        {
            var product = this.productsService.FindProductById(id);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect($"/ShoppingCart/Add/{id}?quantity={quantity}");
        }
    }
}
