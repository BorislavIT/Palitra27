namespace Palitra27.Web.Controllers.Shops
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;
    using Palitra27.Web.ViewModels.Shop;

    public class ShopController : BaseController
    {
        private const int DefaultProductsShow = 12;
        private const int DefaultPage = 1;

        private readonly IShopService shopService;
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;
        private readonly IMapper mapper;

        public ShopController(
            IShopService shopService,
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService,
            IMapper mapper)
        {
            this.shopService = shopService;
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            this.SetUpViewBagForGet();

            var products = this.mapper.Map<List<ProductViewModel>>(this.productsService.FindAllProducts());

            var shopFiltersViewModel = this.CreateShopFiltersViewModel(products);

            return this.View(shopFiltersViewModel);
        }

        [HttpPost]
        public IActionResult Index(ShopViewModel model)
        {
            var paginatedProducts = this.shopService.Find(model).ToList();

            this.SetUpViewBagForPost(model, paginatedProducts);

            var products = this.mapper.Map<List<ProductViewModel>>(this.shopService.Find(model));

            var shopFiltersViewModel = this.CreateShopFiltersViewModel(products);

            return this.View(shopFiltersViewModel);
        }

        [NonAction]
        private void Pagination(List<ProductDTO> products, int show)
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

        private void SetUpViewBagForPost(ShopViewModel model, List<ProductDTO> paginatedProducts)
        {
            this.ViewBag.skipProducts = model.Page * model.Show;
            this.Pagination(paginatedProducts, model.Show);
            this.ViewBag.CurrentPage = model.Page;
            this.ViewBag.Show = model.Show;
            this.ViewBag.Sort = model.Sorting;
        }

        private void SetUpViewBagForGet()
        {
            this.ViewBag.CurrentPage = DefaultPage;
            var paginatedProducts = this.productsService.FindAllProducts();
            this.ViewBag.ProductsToShow = DefaultProductsShow;
            this.Pagination(paginatedProducts, DefaultProductsShow);
        }

        private ShopFiltersViewModel CreateShopFiltersViewModel(List<ProductViewModel> products)
        {
            var productsListViewModel = this.CreateproductListViewModel(products);

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();
            var productCategoriesBrandsViewModel = this.CreateCategoryBrandViewModel(categories, brands);

            var model = new ShopFiltersViewModel { BrandCategoryViewModel = productCategoriesBrandsViewModel, Products = productsListViewModel };
            return model;
        }

        private ProductListViewModel CreateproductListViewModel(List<ProductViewModel> products)
        {
            return new ProductListViewModel { Products = products };
        }

        private CategoryBrandViewModel CreateCategoryBrandViewModel(List<CategoryDTO> categories, List<BrandDTO> brands)
        {
            return new CategoryBrandViewModel { Brands = brands, Categories = categories };
        }
    }
}
