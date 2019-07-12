namespace Palitra27.Web.Controllers.Shops
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Data.Common.Repositories;
    using Palitra27.Data.Models;
    using Palitra27.Services.Data;
    using Palitra27.Services.Mapping;
    using Palitra27.Web.ViewModels.Products;
    using Palitra27.Web.ViewModels.Shop;

    public class ShopController : BaseController
    {
        private readonly IDeletableEntityRepository<Product> repository;
        private readonly IShopService shopService;

        public ShopController(IDeletableEntityRepository<Product> repository, IShopService shopService)
        {
            this.repository = repository;
            this.shopService = shopService;
        }

        public IActionResult Index()
        {
            var products = this.repository.All().To<ProductViewModel>().ToList();
            var model = new ProductListViewModel { Products = products };
            return this.View(model);
        }

        [HttpPost]
        public IActionResult Index(ShopViewModel model)
        {
            var products = this.shopService.Find(model).To<ProductViewModel>().ToList();

            var modelForView = new ProductListViewModel { Products = products };

            return this.View(modelForView);
        }
    }
}