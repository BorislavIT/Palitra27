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

        public CategoriesService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public CategoryDTO CreateCategory(CreateCategoryBindingModel model)
        {
            var category = new Category { Name = model.CategoryName };
            this.context.Categories.Add(category);
            this.context.SaveChanges();
            var dTO = this.mapper.Map<CategoryDTO>(category);
            return dTO;
        }

        public List<CategoryDTO> FindAllCategories()
        {
            var categories = this.context.Categories.ToList();
            var categoriesDto = this.mapper.Map<List<CategoryDTO>>(categories);
            return categoriesDto;
        }
    }
}
