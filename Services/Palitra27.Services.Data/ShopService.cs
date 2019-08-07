namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Web.ViewModels.Shop;

    public class ShopService : IShopService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IProductsService productsService;
        private readonly IMapper mapper;

        public ShopService(
            ApplicationDbContext dbContext,
            IProductsService productsService,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.productsService = productsService;
            this.mapper = mapper;
        }

        public List<ProductDTO> Find(ShopViewModel model)
        {
            var products = this.productsService.FindAllProducts();

            var brand = this.FindBrandByModel(model);

            var category = this.FindCategoryByModel(model);

            if (brand == null && category == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .ToList();
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                        .ToList();
                }
            }
            else if (brand == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(c => c.Category.Name == category.Name)
                        .ToList();
                }
                else
                {
                    products = products
                     .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                     .Where(c => c.Category == category)
                     .ToList();
                }
            }
            else if (category == null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(p => p.Brand.Name == brand.Name)
                        .ToList();
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                        .Where(x => x.Brand.Name == brand.Name)
                        .ToList();
                }
            }

            if (brand != null && category != null)
            {
                if (model.PriceUpper == 0)
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower)
                        .Where(p => p.Category.Name == category.Name && p.Brand.Name == brand.Name)
                        .ToList();
                }
                else
                {
                    products = products
                        .Where(p => p.Price >= model.PriceLower && p.Price <= model.PriceUpper)
                        .Where(p => p.Category.Name == category.Name && p.Brand.Name == brand.Name)
                        .ToList();
                }
            }

            switch (model.Sorting)
            {
                case "pASC":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "pDESC":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                case "nAZ":
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "nZA":
                    products = products.OrderByDescending(p => p.Name).ToList();
                    break;
            }

            return products.ToList();
        }

        private CategoryDTO FindCategoryByModel(ShopViewModel model)
        {
            return this.mapper.Map<CategoryDTO>(this.dbContext.Categories
                .Where(x => x.IsDeleted == false)
                .FirstOrDefault(c => c.Name == model.Category));
        }

        private BrandDTO FindBrandByModel(ShopViewModel model)
        {
            return this.mapper.Map<BrandDTO>(this.dbContext.Brands
                .Where(x => x.IsDeleted == false)
                .FirstOrDefault(b => b.Name == model.Brand));
        }
    }
}
