namespace Palitra27.Web.Areas.Administration.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.Areas.Administration.ViewModels.AdminChooseViewModel;
    using Palitra27.Web.ViewModels.Errors;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A product with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Products/Create";

        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;
        private readonly IMapper mapper;

        public ProductsController(
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService,
            IMapper mapper)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var createProductBrandAndCategoryAndDataViewModel = new CreateProductBrandAndCategoryAndDataViewModel
            {
                BrandCategoryViewModel = productCategoryBrandViewModel,
                CreateProductBindingModel = new CreateProductBindingModel(),
            };

            return this.View(createProductBrandAndCategoryAndDataViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProductBrandAndCategoryAndDataViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var categories = this.categoriesService.FindAllCategories();
                var brands = this.brandsService.FindAllBrands();

                var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };
                model.BrandCategoryViewModel = productCategoryBrandViewModel;
                return this.View(model);
            }

            var product = this.productsService.Create(model.CreateProductBindingModel, model.CreateProductBindingModel.Image);

            if (product == null)
            {
                var creationErrorViewModel = new CreationErrorViewModel { ErrorMessage = CreationAlreadyExistsErrorMessage, HyperLink = HyperLinkForCreationError };
                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect("/Home/Index");
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

            var productModel = this.mapper.Map<ProductInfoViewModel>(product);

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { ProductInfoViewModel = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductEditBindingModel productEditBindingModel)
        {
            ProductDTO product;
            if (!this.ModelState.IsValid)
            {
                product = this.productsService.FindProductById(productEditBindingModel.Id);
            }
            else
            {
                product = this.productsService.EditProduct(productEditBindingModel);
            }

            var productModel = this.mapper.Map<ProductInfoViewModel>(product);

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = new BrandCategoryViewModel { Brands = brands, Categories = categories };

            var model = new ProductEditViewModel { ProductInfoViewModel = productModel, BrandCategoryViewModel = productCategoryBrandViewModel };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult EditDescription(EditDescriptionBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditDescription(editDescriptionBindingModel);

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditSpecifications(EditSpecificationsBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditSpecifications(editDescriptionBindingModel);
            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        public IActionResult ChooseOne(string id)
        {
            var model = new AdminChooseViewModel { Id = id };
            return this.View(model);
        }
    }
}
