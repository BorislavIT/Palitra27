namespace Palitra27.Web.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class UserController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;

        public UserController(
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
        }

        [HttpPost]
        public IActionResult AddReview(AddReviewBindingModel addReviewBindingModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.productsService.AddReview(addReviewBindingModel, userId);
            var product = this.productsService.FindProductById(addReviewBindingModel.Id);
            var productModel = new ProductInfoViewModel { Id = addReviewBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Product/Info/{product.Id}");
        }
    }
}
