namespace Palitra27.Web.ViewModels.Products
{
    using System.Collections.Generic;

    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;

    public class BrandCategoryViewModel
    {
        public List<CategoryDTO> Categories { get; set; }

        public List<BrandDTO> Brands { get; set; }
    }
}
