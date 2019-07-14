namespace Palitra27.Web.Controllers.Admins
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class AdminController : BaseController
    {
        private readonly IProductsService productsService;

        public AdminController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
        public IActionResult Create()
        {
            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            return this.View(productCategoryBrandViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProductBindingModel model)
        {
            this.productsService.Create(model);
            return this.Redirect("/");
        }

        public IActionResult Find()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Find(string id)
        {
            return this.Redirect($"/Admin/Edit/{id}");
        }

        public IActionResult Edit(string id)
        {
            var product = this.productsService.FindProductById(id);

            var productModel = new ProductInfoViewModel { Id = id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, ProductBrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductEditBindingModel productEditBindingModel)
        {
            var product = this.productsService.EditProduct(productEditBindingModel);

            var productModel = new ProductInfoViewModel { Id = productEditBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, ProductBrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult AddReview(AddReviewBindingModel addReviewBindingModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = this.productsService.AddReview(addReviewBindingModel, userId);
            var product = this.productsService.FindProductById(addReviewBindingModel.Id);
            var productModel = new ProductInfoViewModel { Id = addReviewBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, ProductBrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Admin/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditDescription(EditDescriptionBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditDescription(editDescriptionBindingModel);

            var productModel = new ProductInfoViewModel { Id = editDescriptionBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, ProductBrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Admin/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditSpecifications(EditSpecificationsBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditSpecifications(editDescriptionBindingModel);

            var productModel = new ProductInfoViewModel { Id = editDescriptionBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.productsService.FindAllCategories().ToList();
            var brands = this.productsService.FindAllBrands().ToList();

            var productCategoryBrandViewModel = new ProductBrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, ProductBrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Admin/Edit/{product.Id}");
        }
    }
}
