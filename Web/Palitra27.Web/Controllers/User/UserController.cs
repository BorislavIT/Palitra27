namespace Palitra27.Web.Controllers.User
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;
    using Palitra27.Web.ViewModels.Products;

    [Authorize(Roles = GlobalConstants.UserRoleName + "," + GlobalConstants.AdministratorRoleName)]
    public class UserController : BaseController
    {
        private readonly IProductsService productsService;
        private readonly IUserService userService;

        public UserController(
            IProductsService productsService,
            IUserService userService)
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
