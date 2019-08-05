namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Web.ViewModels.Brands;

    public interface IBrandsService
    {
        BrandDTO CreateBrand(CreateBrandBindingModel model);

        List<BrandDTO> FindAllBrands();
    }
}
