namespace Palitra27.Web.Areas.Administration.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.Areas.Administration.ViewModels.AdminChooseViewModel;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsController : AdministrationController
    {
        private const string CreationAlreadyExistsErrorMessage = "A product with such name already exists, ";
        private const string HyperLinkForCreationError = "/Administration/Products/Create";

        private const string ProductDoesntExistErrorMessage = "That product doesn't exist, ";
        private const string HyperLinkForDoesntExistError = "/Shop/Index";

        private readonly IProductsService productsService;
        private readonly ICategoriesService categoriesService;
        private readonly IBrandsService brandsService;
        private readonly IMapper mapper;
        private readonly IErrorService errorService;

        public ProductsController(
            IProductsService productsService,
            ICategoriesService categoriesService,
            IBrandsService brandsService,
            IMapper mapper,
            IErrorService errorService)
        {
            this.productsService = productsService;
            this.categoriesService = categoriesService;
            this.brandsService = brandsService;
            this.mapper = mapper;
            this.errorService = errorService;
        }

        public IActionResult Create()
        {
            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var brandCategoryViewModel = this.brandsService.CreateBrandCategoryViewModelByCategoriesAndBrands(categories, brands);

            var createProductBrandAndCategoryAndDataViewModel = this.CreateProductBrandAndCategoryAndDataViewModel(brandCategoryViewModel, new CreateProductBindingModel());

            return this.View(createProductBrandAndCategoryAndDataViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateProductBrandAndCategoryAndDataViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var categories = this.categoriesService.FindAllCategories();
                var brands = this.brandsService.FindAllBrands();

                var productCategoryBrandViewModel = this.brandsService.CreateBrandCategoryViewModelByCategoriesAndBrands(categories, brands);
                model.BrandCategoryViewModel = productCategoryBrandViewModel;

                return this.View(model);
            }

            var product = this.productsService.Create(model.CreateProductBindingModel, model.CreateProductBindingModel.Image);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(CreationAlreadyExistsErrorMessage, HyperLinkForCreationError);

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

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var productInfoViewModel = this.mapper.Map<ProductInfoViewModel>(product);

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var categoryBrandViewModel = this.brandsService.CreateBrandCategoryViewModelByCategoriesAndBrands(categories, brands);

            var model = this.CreateProductEditViewModel(productInfoViewModel, categoryBrandViewModel);

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

            var productInfoViewModel = this.mapper.Map<ProductInfoViewModel>(product);

            var categories = this.categoriesService.FindAllCategories();
            var brands = this.brandsService.FindAllBrands();

            var productCategoryBrandViewModel = this.brandsService.CreateBrandCategoryViewModelByCategoriesAndBrands(categories, brands);

            var model = this.CreateProductEditViewModel(productInfoViewModel, productCategoryBrandViewModel);

            return this.View(model);
        }

        [HttpPost]
        public IActionResult EditDescription(EditDescriptionBindingModel editDescriptionBindingModel)
        {
            var product = this.productsService.EditDescription(editDescriptionBindingModel);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        [HttpPost]
        public IActionResult EditSpecifications(EditSpecificationsBindingModel editSpecificationsBindingModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect($"/Administration/Products/Edit/{editSpecificationsBindingModel.Id}");
            }

            var product = this.productsService.EditSpecifications(editSpecificationsBindingModel);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            return this.Redirect($"/Administration/Products/Edit/{product.Id}");
        }

        public IActionResult ChooseOne(string id)
        {
            var product = this.productsService.FindProductById(id);

            if (product == null)
            {
                var creationErrorViewModel = this.errorService.CreateCreateionErrorViewModel(ProductDoesntExistErrorMessage, HyperLinkForDoesntExistError);

                return this.RedirectToAction("CreationError", "Error", creationErrorViewModel);
            }

            var model = this.CreateAdminChooseViewModel(id);

            return this.View(model);
        }

        [NonAction]
        private CreateProductBrandAndCategoryAndDataViewModel CreateProductBrandAndCategoryAndDataViewModel(CategoryBrandViewModel brandCategoryViewModel, CreateProductBindingModel createProductBindingModel)
        {
            var productBrandAndCategoryAndDataViewModel = new CreateProductBrandAndCategoryAndDataViewModel { BrandCategoryViewModel = brandCategoryViewModel, CreateProductBindingModel = createProductBindingModel };

            return productBrandAndCategoryAndDataViewModel;
        }

        [NonAction]
        private ProductEditViewModel CreateProductEditViewModel(ProductInfoViewModel productInfoViewModel, CategoryBrandViewModel brandCategoryViewModel)
        {
            var productEditViewModel = new ProductEditViewModel { BrandCategoryViewModel = brandCategoryViewModel, ProductInfoViewModel = productInfoViewModel };

            return productEditViewModel;
        }

        [NonAction]
        private AdminChooseViewModel CreateAdminChooseViewModel(string id)
        {
            var adminChooseViewModel = new AdminChooseViewModel { Id = id };

            return adminChooseViewModel;
        }
    }
}
