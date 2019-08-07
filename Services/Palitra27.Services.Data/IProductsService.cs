namespace Palitra27.Services.Data
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Web.ViewModels.Products;

    public interface IProductsService
    {
        ProductDTO Create(CreateProductBindingModel model, IFormFile image);

        ProductDTO FindProductById(string id);

        ProductDTO EditProduct(ProductEditBindingModel model);

        ReviewDTO AddReview(AddReviewBindingModel model, ApplicationUserDTO user);

        ProductDTO EditDescription(EditDescriptionBindingModel model);

        ProductDTO EditSpecifications(EditSpecificationsBindingModel model);

        List<ProductDTO> FindAllProducts();

        Product FindDomainProduct(string id);
    }
}
