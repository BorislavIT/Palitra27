namespace Palitra27.Web.Controllers.Home
{
    using Microsoft.AspNetCore.Mvc;
    using Palitra27.Services.Data;

    public class HomeController : BaseController
    {
        private readonly IProductsService productsService;

        public HomeController(
            IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
