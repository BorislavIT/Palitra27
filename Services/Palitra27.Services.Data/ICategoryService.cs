namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Categories;

    public interface ICategoryService
    {
        Category CreateCategory(CreateCategoryBindingModel model);
    }
}
