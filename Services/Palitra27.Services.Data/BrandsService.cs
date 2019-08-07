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

            if (checkBrand.IsDeleted == true)
            {
                checkBrand.IsDeleted = false;
                this.context.Brands.Update(checkBrand);
                this.context.SaveChanges();

                return this.mapper.Map<BrandDTO>(checkBrand);
            }

            if (checkBrand != null)
            {
                return null;
            }
            else
            {
                var brand = this.CreateBrandByName(model);

                this.context.Brands.Add(brand);
                this.context.SaveChanges();

                return this.mapper.Map<BrandDTO>(brand);
            }
        }

        public CategoryBrandViewModel CreateBrandCategoryViewModelByCategoriesAndBrands(List<CategoryDTO> categories, List<BrandDTO> brands)
        {
            var brandCategoryViewModel = new CategoryBrandViewModel { Categories = categories, Brands = brands };

            return brandCategoryViewModel;
        }

        public List<BrandDTO> FindAllBrands()
        {
            var brands = this.context.Brands
                .Where(x => x.IsDeleted == false)
                .ToList();

            return this.mapper.Map<List<BrandDTO>>(brands);
        }

        public BrandDTO RemoveBrand(CreateBrandBindingModel model)
        {
            var brand = this.FindBrandByNameAndCheckIsDeleted(model);
            if (brand == null)
            {
                return null;
            }

            brand.IsDeleted = true;

            this.context.Update(brand);
            this.context.SaveChanges();

            return this.mapper.Map<BrandDTO>(brand);
        }

        private Brand CreateBrandByName(CreateBrandBindingModel model)
        {
            var brand = new Brand { Name = model.Name };

            return brand;
        }

        private Brand FindBrandByNameAndCheckIsDeleted(CreateBrandBindingModel model)
        {
            var brand = this.context.Brands
                .Where(x => x.IsDeleted == false)
               .FirstOrDefault(b => b.Name == model.Name);

            if (this.context.Brands.Where(x => x.IsDeleted == false).Count() == 1)
            {
                return null;
            }

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
