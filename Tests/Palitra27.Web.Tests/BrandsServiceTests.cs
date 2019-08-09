namespace Palitra27.Web.Tests
{
    using System;
    using System.Linq;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Brands;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class BrandsServiceTests
    {
        [Fact]
        public void CreateBrandShouldCreateBrand()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateBrandShouldCreateBrand_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString().Substring(0,15);
            var model = new CreateBrandBindingModel { Name = brandName };

            var brand = brandService.CreateBrand(model);

            Assert.True(dbContext.Brands.Count() == 1);
            Assert.True(brand.Name == brandName);
        }

        [Fact]
        public void CreateBrandShouldntCreateBrandAndShouldReturnNullIfAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateBrandShouldntCreateBrandAndShouldReturnNullIfAlreadyExists_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };

            brandService.CreateBrand(model);
            var brand1 = brandService.CreateBrand(model);

            Assert.True(dbContext.Brands.Count() == 1);
            Assert.Null(brand1);
        }

        [Fact]
        public void CreateBrandShouldSetDeletedToFalseIfProductAlreadyExistsAndIsDeletedIsTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateBrandShouldCreateBrand_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var brandName1 = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };
            var model1 = new CreateBrandBindingModel { Name = brandName1 };

            brandService.CreateBrand(model);
            brandService.CreateBrand(model1);

            var brandToRemove = dbContext.Brands.FirstOrDefault(x => x.Name == brandName);
            brandToRemove.IsDeleted = true;
            dbContext.Brands.Update(brandToRemove);
            dbContext.SaveChanges();

            var brand1 = brandService.CreateBrand(model);

            Assert.NotNull(brand1);
            Assert.True(brand1.IsDeleted == false);
        }

        [Fact]
        public void CreateBrandCategoryViewModelByCategoriesAndBrandsShouldCreateBrandCategoryViewModelByCategoriesAndBrands()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateBrandCategoryViewModelByCategoriesAndBrandsShouldCreateBrandCategoryViewModelByCategoriesAndBrands_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            this.SeedDbWithCategories(dbContext);
            this.SeeDbdWithBrands(dbContext);

            var brands = brandService.FindAllBrands();
            var categories = categoriesService.FindAllCategories();

            var model = new CategoryBrandViewModel { Brands = brands, Categories = categories };

            Assert.True(model.Brands.Count == 3 && model.Categories.Count == 3);
        }

        [Fact]
        public void FindAllBrandsShouldReturnAllBrands()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllBrandsShouldReturnAllBrands_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            this.SeeDbdWithBrands(dbContext);

            var brands = brandService.FindAllBrands();

            Assert.True(brands.Count == 3);
        }

        [Fact]
        public void FindAllBrandsShouldReturnOnlyBrandsWhereIsDeletedIsFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllBrandsShouldReturnOnlyBrandsWhereIsDeletedIsFalse_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var brandName1 = Guid.NewGuid().ToString();
            var brandName2 = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };
            var model1 = new CreateBrandBindingModel { Name = brandName1 };
            var model2 = new CreateBrandBindingModel { Name = brandName2 };

            brandService.CreateBrand(model);
            brandService.CreateBrand(model1);
            brandService.CreateBrand(model2);
            brandService.RemoveBrand(model);
            brandService.RemoveBrand(model1);

            var brands = brandService.FindAllBrands();

            Assert.True(brands.Count == 1);
        }

        [Fact]
        public void RemoveBrandShouldSetIsDeletedToTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveBrandShouldSetIsDeletedToTrue_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var brandName1 = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };
            var model1 = new CreateBrandBindingModel { Name = brandName1 };

            brandService.CreateBrand(model);
            brandService.CreateBrand(model1);
            brandService.RemoveBrand(model);
            var brand1 = brandService.CreateBrand(model);

            Assert.NotNull(brand1);
            Assert.True(brand1.IsDeleted == false);
        }

        [Fact]
        public void RemoveBrandShouldReturnNullIfBrandsCountIs1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveBrandShouldReturnNullIfBrandsCountIs1_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };

            brandService.CreateBrand(model);
            var brand = brandService.RemoveBrand(model);

            Assert.Null(brand);
        }

        [Fact]
        public void RemoveBrandShouldReturnNullIfBrandIsNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RRemoveBrandShouldReturnNullIfBrandIsNull_Brand_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var brandService = new BrandsService(dbContext, mapper);

            var brandName = Guid.NewGuid().ToString();
            var model = new CreateBrandBindingModel { Name = brandName };

            var brand = brandService.RemoveBrand(model);

            Assert.Null(brand);
        }

        private void SeedDbWithCategories(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Categories.Add(new Category { Name = Guid.NewGuid().ToString().Substring(0, 15) });
                dbContext.SaveChanges();
            }
        }

        private void SeeDbdWithBrands(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Brands.Add(new Brand { Name = Guid.NewGuid().ToString().Substring(0, 15) });
                dbContext.SaveChanges();
            }
        }

        private IMapper SetUpAutoMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });

            return mockMapper.CreateMapper();
        }
    }
}
