namespace Palitra27.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Web.ViewModels.FavouriteList;

    public class FavouritesService : IFavouritesService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IProductsService productsService;
        private readonly IUsersService userService;
        private readonly IMapper mapper;

        public FavouritesService(
            ApplicationDbContext dbContext,
            IProductsService productsService,
            IUsersService userService,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.productsService = productsService;
            this.userService = userService;
            this.mapper = mapper;
        }

        public void AddProduct(string id, string username)
        {
            var user = this.userService.FindUserByUsername(username);

            var product = this.productsService.FindDomainProduct(id);

            if (this.ProductOrUserIsNullOrFavouriteProductIsntNull(user, product))
            {
                return;
            }

            var favouriteList = this.FindFavouriteListByUserId(user);

            var favouriteProductToAdd = this.CreateFavouriteProduct(product, favouriteList);

            this.dbContext.FavouriteProducts.Add(favouriteProductToAdd);
            favouriteList.FavouriteProducts.Add(favouriteProductToAdd);
            this.dbContext.FavouriteLists.Update(favouriteList);
            this.dbContext.SaveChanges();
        }

        public List<FavouriteProductViewModel> AllFavouriteProducts(string username)
        {
            var user = this.userService.FindUserByUsername(username);

            var favouriteList = this.FindFavouriteListByUserId(user);
            var favouriteProducts = this.FindFavouriteListProducts(favouriteList);

            return this.mapper.Map<List<FavouriteProductViewModel>>(favouriteProducts);
        }

        public void RemoveProduct(string id, string username)
        {
            var user = this.userService.FindUserByUsername(username);

            var product = this.productsService.FindDomainProduct(id);

            var favouriteProduct = this.FindFavouriteProductByProductId(product);

            if (this.ProductOrUserOrFavouriteProductIsNull(user, product, favouriteProduct))
            {
                return;
            }
            else
            {
                var favouriteList = this.FindFavouriteListByProductId(favouriteProduct);
                favouriteList.FavouriteProducts.Remove(favouriteProduct);
                this.dbContext.FavouriteLists.Update(favouriteList);

                this.dbContext.FavouriteProducts.Remove(favouriteProduct);
                this.dbContext.SaveChanges();
            }
        }

        private bool ProductOrUserIsNullOrFavouriteProductIsntNull(ApplicationUserDTO user, Product product)
        {

            if (product == null || user == null || this.dbContext.FavouriteProducts.FirstOrDefault(x => x.ProductId == product.Id) != null)
            {
                return true;
            }

            return false;
        }

        private bool ProductOrUserOrFavouriteProductIsNull(ApplicationUserDTO user, Product product, FavouriteProduct favouriteProduct)
        {
            if (product == null || user == null || favouriteProduct == null)
            {
                return true;
            }

            return false;
        }

        private FavouriteProduct CreateFavouriteProduct(Product product, FavouriteList favouriteList)
        {
            var favouriteProduct = new FavouriteProduct
            {
                Product = product,
                ProductId = product.Id,
                FavouriteList = favouriteList,
                FavouriteListId = favouriteList.Id,
            };

            return favouriteProduct;
        }

        private FavouriteList FindFavouriteListByUserId(ApplicationUserDTO user)
        {
            var favouriteList = this.dbContext.FavouriteLists
                .FirstOrDefault(x => x.UserId == user.Id);

            return favouriteList;
        }

        private FavouriteProduct FindFavouriteProductByProductId(Product product)
        {
            var favouriteProduct = this.dbContext.FavouriteProducts
                .FirstOrDefault(x => x.ProductId == product.Id);

            return favouriteProduct;
        }

        private List<FavouriteProduct> FindFavouriteListProducts(FavouriteList favouriteList)
        {
            var favouriteProducts = this.dbContext.FavouriteProducts
                .Include(x => x.Product)
                .Include(x => x.FavouriteList)
                .Where(x => x.FavouriteList.Id == favouriteList.Id)
                .ToList();

            return favouriteProducts;
        }

        private FavouriteList FindFavouriteListByProductId(FavouriteProduct favouriteProduct)
        {
            var favouriteList = this.dbContext.FavouriteLists
                    .Include(x => x.FavouriteProducts)
                    .FirstOrDefault(x => x.Id == favouriteProduct.FavouriteListId);

            return favouriteList;
        }
    }
}
