namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.FavouriteList;

    public class FavouritesService : IFavouritesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IProductsService productsService;
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public FavouritesService(
            ApplicationDbContext dbContext,
            IProductsService productsService,
            IUserService userService,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.productsService = productsService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public void AddProduct(string id, string username)
        {
            var user = this.userService
                .GetUserByUsername(username);

            var product = this.productsService
                .FindDomainProduct(id);

            var favouriteList = this.dbContext.FavouriteLists
                 .FirstOrDefault(x => x.UserId == user.Id);

            var favouriteProduct = this.dbContext
                .FavouriteProducts
                .FirstOrDefault(x => x.ProductId == product.Id);

            if (product == null || user == null || favouriteProduct != null)
            {
                return;
            }
            else
            {
                var favouriteProductToAdd = new FavouriteProduct
                {
                    Product = product,
                    ProductId = product.Id,
                    FavouriteList = favouriteList,
                    FavouriteListId = favouriteList.Id,
                };

                this.dbContext.FavouriteProducts.Add(favouriteProductToAdd);
                favouriteList.FavouriteProducts.Add(favouriteProductToAdd);
                this.dbContext.FavouriteLists.Update(favouriteList);
                this.dbContext.SaveChanges();
            }
        }

        public List<FavouriteProductViewModel> AllFavouriteProducts(string username)
        {
            var user = this.userService
                .GetUserByUsername(username);

            var favouriteList = this.dbContext.FavouriteLists
             .FirstOrDefault(x => x.User.Id == user.Id);

            var favouriteProducts = this.dbContext.FavouriteProducts
                .Include(x => x.Product)
                .Include(x => x.FavouriteList)
                .Where(x => x.FavouriteList.Id == favouriteList.Id)
                .ToList();

            var dsa = this.mapper.Map<List<FavouriteProductViewModel>>(favouriteProducts);
            return dsa;
        }

        public void RemoveProduct(string productId, string username)
        {
            var user = this.userService
               .GetUserByUsername(username);

            var product = this.productsService
               .FindDomainProduct(productId);

            var favouriteProduct = this.dbContext
                .FavouriteProducts
                .FirstOrDefault(x => x.ProductId == product.Id);

            if (product == null || user == null || favouriteProduct == null)
            {
                return;
            }
            else
            {
                var favouriteList = this.dbContext.FavouriteLists
                    .Include(x => x.FavouriteProducts)
                    .FirstOrDefault(x => x.Id == favouriteProduct.FavouriteListId);
                favouriteList.FavouriteProducts.Remove(favouriteProduct);
                this.dbContext.FavouriteLists.Update(favouriteList);

                this.dbContext.FavouriteProducts.Remove(favouriteProduct);
                this.dbContext.SaveChanges();
            }
        }
    }
}
