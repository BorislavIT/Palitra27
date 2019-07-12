namespace Palitra27.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Palitra27.Data.Models;

    public interface ICategoryService
    {
        Category GetCategoryByName(string name);

        Category GetCategoryById(string id);
    }
}
