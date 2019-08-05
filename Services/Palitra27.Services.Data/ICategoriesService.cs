namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Web.ViewModels.Categories;

    public interface ICategoriesService
    {
        CategoryDTO CreateCategory(CreateCategoryBindingModel model);

        List<CategoryDTO> FindAllCategories();
    }
}
