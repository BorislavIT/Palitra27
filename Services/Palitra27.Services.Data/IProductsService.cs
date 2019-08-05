namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Web.ViewModels.Products;

    public interface IProductsService
    {
        ProductDTO Create(CreateProductBindingModel model);

        ProductDTO Create(CreateProductBindingModel model, IFormFile image);

        ProductDTO FindProductById(string id);

        ProductDTO EditProduct(ProductEditBindingModel model);

        ReviewDTO AddReview(AddReviewBindingModel model, string userId);

        ProductDTO EditDescription(EditDescriptionBindingModel model);

        ProductDTO EditSpecifications(EditSpecificationsBindingModel model);

        ProductDTO GetOnlyProductById(string id);

        List<ProductDTO> GetAllProducts();

        Product FindDomainProduct(string id);
    }
}
