namespace Palitra27.Web.Controllers.User
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class UserController : BaseController
    {
        private readonly IProductsService productsService;

        public UserController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpPost]
        public IActionResult AddReview(AddReviewBindingModel addReviewBindingModel)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.productsService.AddReview(addReviewBindingModel, userId);

            var product = this.productsService.FindProductById(addReviewBindingModel.Id);

            return this.Redirect($"/Products/Info/{product.Id}");
        }
    }
}
