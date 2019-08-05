namespace Palitra27.Web.Controllers.Favourite
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Common;
    using Palitra27.Services.Data;

    [Authorize(Roles = GlobalConstants.UserRoleName + "," + GlobalConstants.AdministratorRoleName)]
    public class FavouriteController : Controller
    {
        private readonly IFavouritesService favouritesService;

        public FavouriteController(IFavouritesService favouritesService)
        {
            this.favouritesService = favouritesService;
        }

        public IActionResult Index()
        {
            var username = this.User.Identity.Name;
            var taskModel = this.favouritesService.AllFavouriteProducts(username);
            var model = taskModel.Result;
            if (model.Count == 0)
            {
                return this.Redirect("/Home/Index");
            }
            else
            {
                return this.View(model);
            }
        }

        public IActionResult Delete(string productId)
        {
            this.favouritesService.RemoveProduct(productId, this.User.Identity.Name);
            return this.RedirectToAction("Index");
        }

        public IActionResult Add(string productId)
        {
            this.favouritesService.AddProduct(productId, this.User.Identity.Name);
            return this.RedirectToAction("Index");
        }
    }
}
