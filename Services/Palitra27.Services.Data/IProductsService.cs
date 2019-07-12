namespace Palitra27.Services.Data
{
    using System.Linq;

    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public interface IProductsService
    {
        void Create(CreateProductBindingModel model);

        IQueryable<Category> FindAllCategories();

        IQueryable<ProductBrand> FindAllBrands();
    }
}
