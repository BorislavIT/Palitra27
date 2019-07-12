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
        private readonly IProductsService productsService;

        public ShopController(IDeletableEntityRepository<Product> repository, IShopService shopService, IProductsService productsService)
        {
            this.repository = repository;
            this.shopService = shopService;
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            var products = this.repository.All().To<ProductViewModel>().ToList();
            var productsListViewModel = new ProductListViewModel { Products = products };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();
            var productCategoriesBrandsViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ShopFiltersViewModel { ProductBrandCategoryViewModel = productCategoriesBrandsViewModel, Products = productsListViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Index(ProductCategoryBrandViewModel model)
        {
            var products = this.repository.All().Where(p => p.Brand.Name == model.Brand && p.Category.Name == model.Category).To<ProductViewModel>().ToList();

            var productsListViewModel = new ProductListViewModel { Products = products };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();
            var productCategoriesBrandsViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var shopFiltersViewModel = new ShopFiltersViewModel { ProductBrandCategoryViewModel = productCategoriesBrandsViewModel, Products = productsListViewModel };

            return this.View(shopFiltersViewModel);
        }
    }
}