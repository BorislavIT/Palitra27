namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Web.ViewModels.Brands;
    using Palitra27.Web.ViewModels.Products;

    public class BrandsService : IBrandsService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public BrandsService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public BrandDTO CreateBrand(CreateBrandBindingModel model)
        {
            var checkBrand = this.FindBrandByName(model);
            if (checkBrand != null)
            {
                return null;
            }

            var brand = this.CreateBrandByName(model);

            this.context.Brands.Add(brand);
            this.context.SaveChanges();

            return this.mapper.Map<BrandDTO>(brand);
        }

        public CategoryBrandViewModel CreateBrandCategoryViewModelByCategoriesAndBrands(List<CategoryDTO> categories, List<BrandDTO> brands)
        {
            var brandCategoryViewModel = new CategoryBrandViewModel { Categories = categories, Brands = brands };

            return brandCategoryViewModel;
        }

        public List<BrandDTO> FindAllBrands()
        {
            var brands = this.context.Brands.ToList();
            return this.mapper.Map<List<BrandDTO>>(brands);
        }

        private Brand CreateBrandByName(CreateBrandBindingModel model)
        {
            var brand = new Brand { Name = model.Name };

            return brand;
        }

        private Brand FindBrandByName(CreateBrandBindingModel model)
        {
            var brand = this.context.Brands
               .FirstOrDefault(b => b.Name == model.Name);

            return brand;
        }
    }
}
