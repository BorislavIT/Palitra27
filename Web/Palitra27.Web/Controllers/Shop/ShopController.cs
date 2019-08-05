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
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;

        public ShopController(
            IDeletableEntityRepository<Product> repository,
            IShopService shopService,
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService)
        {
            this.repository = repository;
            this.shopService = shopService;
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
        }

        public IActionResult Index()
        {
            int show = 12;
            int page = 1;
            this.ViewBag.CurrentPage = page;
            var paginatedProducts = this.repository.All().ToList();
            this.ViewBag.ProductsToShow = show;
            this.Pagination(paginatedProducts, show);

            var products = this.repository.All().To<ProductViewModel>().ToList();
            var productsListViewModel = new ProductListViewModel { Products = products };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();
            var productCategoriesBrandsViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ShopFiltersViewModel { BrandCategoryViewModel = productCategoriesBrandsViewModel, Products = productsListViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Index(ShopViewModel model)
        {
            var products = this.shopService.Find(model).To<ProductViewModel>().ToList();
            var paginatedProducts = this.shopService.Find(model).ToList();
            this.ViewBag.skipProducts = model.Page * model.Show;
            this.Pagination(paginatedProducts, model.Show);
            this.ViewBag.CurrentPage = model.Page;
            this.ViewBag.Show = model.Show;
            this.ViewBag.Sort = model.Sorting;

            var productsListViewModel = new ProductListViewModel { Products = products };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();
            var productCategoriesBrandsViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var shopFiltersViewModel = new ShopFiltersViewModel { BrandCategoryViewModel = productCategoriesBrandsViewModel, Products = productsListViewModel };

            return this.View(shopFiltersViewModel);
        }

        [NonAction]
        public void Pagination(List<Product> products, int show)
        {
            var productsCount = products.Count;

            var pages = 1;
            var productsToShow = show;
            var lastPageProducts = 0;

            if (productsCount <= productsToShow)
            {
                this.ViewBag.ProductsToShow = productsToShow;
                this.ViewBag.Pages = pages;
                this.ViewBag.LastPageProducts = lastPageProducts;
            }
            else if (productsCount > productsToShow)
            {
                if (productsCount % productsToShow == 0)
                {
                    pages = productsCount / productsToShow;
                    this.ViewBag.ProductsToShow = productsToShow;
                    this.ViewBag.Pages = pages;
                    this.ViewBag.LastPageProducts = lastPageProducts;
                }
                else
                {
                    pages = (int)(productsCount / productsToShow);
                    pages++;
                    lastPageProducts = productsCount % productsToShow;
                    this.ViewBag.ProductsToShow = productsToShow;
                    this.ViewBag.Pages = pages;
                    this.ViewBag.LastPageProducts = lastPageProducts;
                }
            }
        }
    }
}
