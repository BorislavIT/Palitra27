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
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public BrandsService(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public BrandDTO CreateBrand(CreateBrandBindingModel model)
        {
            var checkBrand = this.FindBrandByName(model);

            if (checkBrand.IsDeleted == true)
            {
                checkBrand.IsDeleted = false;
                this.dbContext.Brands.Update(checkBrand);
                this.dbContext.SaveChanges();

                return this.mapper.Map<BrandDTO>(checkBrand);
            }

            if (checkBrand != null)
            {
                return null;
            }
            else
            {
                var brand = this.CreateBrandByName(model);

                this.dbContext.Brands.Add(brand);
                this.dbContext.SaveChanges();

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
            var brands = this.dbContext.Brands
                .Where(x => x.IsDeleted == false)
                .ToList();

            return this.mapper.Map<List<BrandDTO>>(brands);
        }

        public BrandDTO RemoveBrand(CreateBrandBindingModel model)
        {
            var brand = this.FindBrandByModelAndCheckIsDeleted(model);
            if (brand == null)
            {
                return null;
            }

            brand.IsDeleted = true;

            this.dbContext.Update(brand);
            this.dbContext.SaveChanges();

            return this.mapper.Map<BrandDTO>(brand);
        }

        private Brand CreateBrandByName(CreateBrandBindingModel model)
        {
            var brand = new Brand { Name = model.Name };

            return brand;
        }

        private Brand FindBrandByModelAndCheckIsDeleted(CreateBrandBindingModel model)
        {
            var brand = this.dbContext.Brands
                .Where(x => x.IsDeleted == false)
               .FirstOrDefault(b => b.Name == model.Name);

            if (this.dbContext.Brands.Where(x => x.IsDeleted == false).Count() == 1)
            {
                return null;
            }

            return brand;
        }

        private Brand FindBrandByName(CreateBrandBindingModel model)
        {
            var brand = this.dbContext.Brands
               .FirstOrDefault(b => b.Name == model.Name);

            return brand;
        }
    }
}
