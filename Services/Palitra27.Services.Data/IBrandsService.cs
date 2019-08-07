namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Web.ViewModels.Brands;
    using Palitra27.Web.ViewModels.Products;

    public interface IBrandsService
    {
        BrandDTO CreateBrand(CreateBrandBindingModel model);

        List<BrandDTO> FindAllBrands();

        CategoryBrandViewModel CreateBrandCategoryViewModelByCategoriesAndBrands(List<CategoryDTO> categories, List<BrandDTO> brands);
    }
}
