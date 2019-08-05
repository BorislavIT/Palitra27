namespace Palitra27.Web.Areas.Administration.Controllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.Areas.Administration.ViewModels;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsController : AdministrationController
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;

        public ProductsController(IProductsService productsService, ICategoriesService categoriesService, IBrandsService brandsService)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
        }

        public IActionResult Create()
        {
            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            return this.View(productCategoryBrandViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProductBindingModel model)
        {
            var product = this.productsService.Create(model, model.Image);
            return this.Redirect("/");
        }

        public IActionResult Find()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Find(string id)
        {
            return this.Redirect($"/Administration/Products/Edit/{id}");
        }

        public IActionResult Edit(string id)
        {
            var product = this.productsService.FindProductById(id);

            var productModel = new ProductInfoViewModel { Id = id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductEditBindingModel productEditBindingModel)
        {
            var product = this.productsService.EditProduct(productEditBindingModel);

            var productModel = new ProductInfoViewModel { Id = productEditBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult AddReview(AddReviewBindingModel addReviewBindingModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var review = this.productsService.AddReview(addReviewBindingModel, userId);
            var product = this.productsService.FindProductById(addReviewBindingModel.Id);

            var productModel = new ProductInfoViewModel { Id = addReviewBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditDescription(EditDescriptionBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditDescription(editDescriptionBindingModel);

            var productModel = new ProductInfoViewModel { Id = editDescriptionBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditSpecifications(EditSpecificationsBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditSpecifications(editDescriptionBindingModel);

            var productModel = new ProductInfoViewModel { Id = editDescriptionBindingModel.Id, Image = product.Image, Name = product.Name, Price = product.Price, Category = product.Category.Name, Brand = product.Brand.Name, Reviews = product.Reviews, Width = product.Width, Depth = product.Depth, Height = product.Height, Weight = product.Weight, Description = product.Description, MiniDescription = product.MiniDescription };

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { Product = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        public IActionResult ChooseOne(string id)
        {
            var model = new AdminChooseViewModel { Id = id };
            return this.View(model);
        }
    }
}
