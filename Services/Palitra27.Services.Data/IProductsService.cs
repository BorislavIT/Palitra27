namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Http;
    using Palitra27.Common.DtoModels.Product;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public interface IProductsService
    {
        ProductDTO Create(CreateProductBindingModel model);

        ProductDTO Create(CreateProductBindingModel model, IFormFile image);

        ProductDTO FindProductById(string id);

        IQueryable<Category> FindAllCategories();

        IQueryable<ProductBrand> FindAllBrands();

        ProductDTO EditProduct(ProductEditBindingModel model);

        Review AddReview(AddReviewBindingModel model, string userId);

        ProductDTO EditDescription(EditDescriptionBindingModel model);

        ProductDTO EditSpecifications(EditSpecificationsBindingModel model);

        ProductDTO GetOnlyProductById(string id);

        IEnumerable<ProductDTO> GetAllProducts();

        List<ProductDTO> FindAllProductsByQuery(string query);

        Product FindDomainProduct(string id);

        //void AddImageUrls(string id, IEnumerable<string> imageUrls);
    }
}
