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
    using Palitra27.Web.ViewModels.Categories;
    using Xunit;

    public class CategoriesServiceTests
    {
        [Fact]
        public void CreateCategoryShouldCreateCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateCategoryShouldCreateCategory_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString().Substring(0, 15);
            var model = new CreateCategoryBindingModel { Name = categoryName };

            var category = categoriesService.CreateCategory(model);

            Assert.True(dbContext.Categories.Count() == 1);
            Assert.True(category.Name == categoryName);
        }

        [Fact]
        public void CreateCategoryShouldntCreateCategoryAndShouldReturnNullIfAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateCategoryShouldntCreateCategoryAndShouldReturnNullIfAlreadyExists_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };

            categoriesService.CreateCategory(model);
            var category1 = categoriesService.CreateCategory(model);

            Assert.True(dbContext.Categories.Count() == 1);
            Assert.Null(category1);
        }

        [Fact]
        public void CreateCategoryShouldSetDeletedToFalseIfProductAlreadyExistsAndIsDeletedIsTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateCategoryShouldCreateCategory_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var categoryName1 = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };
            var model1 = new CreateCategoryBindingModel { Name = categoryName1 };

            categoriesService.CreateCategory(model);
            categoriesService.CreateCategory(model1);

            var categoryToRemove = dbContext.Categories.FirstOrDefault(x => x.Name == categoryName);
            categoryToRemove.IsDeleted = true;
            dbContext.Categories.Update(categoryToRemove);
            dbContext.SaveChanges();

            var category1 = categoriesService.CreateCategory(model);

            Assert.NotNull(category1);
            Assert.True(category1.IsDeleted == false);
        }

        [Fact]
        public void FindAllCategoriesShouldReturnAllCategories()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllCategoriesShouldReturnAllCategories_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            this.SeeDbdWithCategories(dbContext);

            var categories = categoriesService.FindAllCategories();

            Assert.True(categories.Count == 3);
        }

        [Fact]
        public void FindAllCategoriesShouldReturnOnlyCategoriesWhereIsDeletedIsFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllCategoriesShouldReturnOnlyCategoriesWhereIsDeletedIsFalse_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var categoryName1 = Guid.NewGuid().ToString();
            var categoryName2 = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };
            var model1 = new CreateCategoryBindingModel { Name = categoryName1 };
            var model2 = new CreateCategoryBindingModel { Name = categoryName2 };

            categoriesService.CreateCategory(model);
            categoriesService.CreateCategory(model1);
            categoriesService.CreateCategory(model2);
            categoriesService.RemoveCategory(model);
            categoriesService.RemoveCategory(model1);

            var categories = categoriesService.FindAllCategories();

            Assert.True(categories.Count == 1);
        }

        [Fact]
        public void RemoveCategoryShouldSetIsDeletedToTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveCategoryShouldSetIsDeletedToTrue_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var categoryName1 = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };
            var model1 = new CreateCategoryBindingModel { Name = categoryName1 };

            categoriesService.CreateCategory(model);
            categoriesService.CreateCategory(model1);
            categoriesService.RemoveCategory(model);
            var category1 = categoriesService.CreateCategory(model);

            Assert.NotNull(category1);
            Assert.True(category1.IsDeleted == false);
        }

        [Fact]
        public void RemoveCategoryShouldReturnNullIfCategoriesCountIs1()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RemoveCategoryShouldReturnNullIfCategoriesCountIs1_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };

            categoriesService.CreateCategory(model);
            var category = categoriesService.RemoveCategory(model);

            Assert.Null(category);
        }

        [Fact]
        public void RemoveCategoryShouldReturnNullIfCategoryIsNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"RRemoveCategoryShouldReturnNullIfCategoryIsNull_Category_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);

            var mapper = this.SetUpAutoMapper();

            var categoriesService = new CategoriesService(dbContext, mapper);

            var categoryName = Guid.NewGuid().ToString();
            var model = new CreateCategoryBindingModel { Name = categoryName };

            var category = categoriesService.RemoveCategory(model);

            Assert.Null(category);
        }

        private void SeeDbdWithCategories(ApplicationDbContext dbContext)
        {
            for (int i = 0; i < 3; i++)
            {
                dbContext.Categories.Add(new Category { Name = Guid.NewGuid().ToString().Substring(0, 15) });
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
