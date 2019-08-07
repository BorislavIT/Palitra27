namespace Palitra27.Web.Components
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class SearchBarComponent : ViewComponent
    {
        private readonly IProductsService productsService;

        public SearchBarComponent(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IViewComponentResult Invoke()
        {
            var model = this.productsService.FindAllProducts();

            if (model == null || model.Count == 0)
            {
                return this.View(new List<ProductViewModel>());
            }

            return this.View(model.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
            }).ToList());
        }
    }
}
