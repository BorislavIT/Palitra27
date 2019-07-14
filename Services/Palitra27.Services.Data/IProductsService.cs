namespace Palitra27.Services.Data
{
    using System.Linq;

    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public interface IProductsService
    {
        void Create(CreateProductBindingModel model);

        Product FindProductById(string id);

        IQueryable<Category> FindAllCategories();

        IQueryable<ProductBrand> FindAllBrands();

        Product EditProduct(ProductEditBindingModel model);

        Review AddReview(AddReviewBindingModel model, string userId);

        Product EditDescription(EditDescriptionBindingModel model);

        Product EditSpecifications(EditSpecificationsBindingModel model);
    }
}
