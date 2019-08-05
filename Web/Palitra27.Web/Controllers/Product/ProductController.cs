namespace Palitra27.Web.Controllers.Product
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class ProductController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IMapper mapper;
        public ProductController(IProductsService productsService, IMapper mapper)
        {
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public IActionResult Info(string id)
        {
            var product = this.productsService.FindProductById(id);

            var productDTO = this.mapper.Map<ProductDTO>(product);

            var productModel = this.mapper.Map<ProductInfoViewModel>(product);

            return this.View(productModel);
        }

        [HttpPost]
        public IActionResult Info(string id, int qty)
        {
            return this.Redirect($"/ShoppingCart/Add/{id}?qty={qty}");
        }
    }
}
