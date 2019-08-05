namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Web.ViewModels.Brands;

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
            var checkBrand = this.context.Brands
                .FirstOrDefault(x => x.Name == model.Name);
            if (checkBrand != null)
            {
                return null;
            }

            var brand = new Brand { Name = model.Name };

            this.context.Brands.Add(brand);
            this.context.SaveChanges();

            return this.mapper.Map<BrandDTO>(brand);
        }

        public List<BrandDTO> FindAllBrands()
        {
            var brands = this.context.Brands.ToList();
            return this.mapper.Map<List<BrandDTO>>(brands);
        }
    }
}
