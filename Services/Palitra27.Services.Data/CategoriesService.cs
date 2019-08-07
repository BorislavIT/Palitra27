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
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public CategoriesService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public CategoryDTO CreateCategory(CreateCategoryBindingModel model)
        {
            var checkCategory = this.FindCategoryByName(model);

            if (checkCategory.IsDeleted == true)
            {
                checkCategory.IsDeleted = false;
                this.context.Categories.Update(checkCategory);
                this.context.SaveChanges();

                return this.mapper.Map<CategoryDTO>(checkCategory);
            }

            if (checkCategory != null)
            {
                return null;
            }

            var category = this.CreateCategoryByName(model);

            this.context.Categories.Add(category);
            this.context.SaveChanges();

            return this.mapper.Map<CategoryDTO>(category);
        }

        public List<CategoryDTO> FindAllCategories()
        {
            var categories = this.context.Categories
                .Where(x => x.IsDeleted == false)
                .ToList();

            return this.mapper.Map<List<CategoryDTO>>(categories);
        }

        public CategoryDTO RemoveCategory(CreateCategoryBindingModel model)
        {
            var category = this.FindCategoryByNameAndCheckIsDeleted(model);
            if (category == null)
            {
                return null;
            }

            category.IsDeleted = true;

            this.context.Update(category);
            this.context.SaveChanges();

            return this.mapper.Map<CategoryDTO>(category);
        }

        private Category CreateCategoryByName(CreateCategoryBindingModel model)
        {
            var category = new Category { Name = model.Name };

            return category;
        }

        private Category FindCategoryByName(CreateCategoryBindingModel model)
        {
            var category = this.context.Categories
               .FirstOrDefault(c => c.Name == model.Name);

            return category;
        }

        private Category FindCategoryByNameAndCheckIsDeleted(CreateCategoryBindingModel model)
        {
            var category = this.context.Categories
                .Where(x => x.IsDeleted == false)
             .FirstOrDefault(c => c.Name == model.Name);

            if (this.context.Categories.Where(x => x.IsDeleted == false).Count() == 1)
            {
                return null;
            }

            return category;
        }
    }
}
