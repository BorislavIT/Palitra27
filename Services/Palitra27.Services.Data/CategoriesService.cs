namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Web.ViewModels.Categories;

    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public CategoriesService(
            ApplicationDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public CategoryDTO CreateCategory(CreateCategoryBindingModel model)
        {
            var checkCategory = this.FindCategoryByName(model);

            if (checkCategory != null)
            {
                if (checkCategory.IsDeleted == true)
                {
                    checkCategory.IsDeleted = false;
                    this.dbContext.Categories.Update(checkCategory);
                    this.dbContext.SaveChanges();

                    return this.mapper.Map<CategoryDTO>(checkCategory);
                }
                else
                {
                    return null;
                }
            }

            var category = this.CreateCategoryByName(model);

            this.dbContext.Categories.Add(category);
            this.dbContext.SaveChanges();

            return this.mapper.Map<CategoryDTO>(category);
        }

        public List<CategoryDTO> FindAllCategories()
        {
            var categories = this.dbContext.Categories
                .Where(x => x.IsDeleted == false)
                .ToList();

            return this.mapper.Map<List<CategoryDTO>>(categories);
        }

        public Category FindBrandByNameAndCheckIsDeleted(string name)
        {
            throw new System.NotImplementedException();
        }

        public CategoryDTO RemoveCategory(CreateCategoryBindingModel model)
        {
            var category = this.FindCategoryByModelAndCheckIsDeleted(model);
            if (category == null)
            {
                return null;
            }

            category.IsDeleted = true;

            this.dbContext.Update(category);
            this.dbContext.SaveChanges();

            return this.mapper.Map<CategoryDTO>(category);
        }

        public Category FindCategoryByNameAndCheckIsDeleted(CreateCategoryBindingModel model)
        {
            var category = this.dbContext.Categories
                .Where(x => x.IsDeleted == false)
             .FirstOrDefault(c => c.Name == model.Name);

            return category;
        }

        private Category CreateCategoryByName(CreateCategoryBindingModel model)
        {
            var category = new Category { Name = model.Name };

            return category;
        }

        private Category FindCategoryByName(CreateCategoryBindingModel model)
        {
            var category = this.dbContext.Categories
               .FirstOrDefault(c => c.Name == model.Name);

            return category;
        }

        private Category FindCategoryByModelAndCheckIsDeleted(CreateCategoryBindingModel model)
        {
            var category = this.dbContext.Categories
                .Where(x => x.IsDeleted == false)
             .FirstOrDefault(c => c.Name == model.Name);

            if (this.dbContext.Categories.Where(x => x.IsDeleted == false).Count() == 1)
            {
                return null;
            }

            return category;
        }
    }
}
