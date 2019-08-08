namespace Palitra27.Web.Controllers.User
{
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    public class UserController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IUsersService userService;

        public UserController(
            IProductsService productsService,
            IUsersService userService)
        {
            this.productsService = productsService;
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult AddReview(AddReviewBindingModel addReviewBindingModel)
        {
            var user = this.userService.FindUserByUsername(this.User.Identity.Name);
            this.productsService.AddReview(addReviewBindingModel, user);

            var product = this.productsService.FindProductById(addReviewBindingModel.Id);

            return this.Redirect($"/Products/Info/{product.Id}");
        }
    }
}
