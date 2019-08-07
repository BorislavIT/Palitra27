namespace Palitra27.Web.MappingConfigurations
{
    using System;

    using AutoMapper;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.ApplicationUserDTO;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Data.Models.DtoModels.Order;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Data.Models.DtoModels.Review;
    using Palitra27.Data.Models.DtoModels.ShoppingCart;
    using Palitra27.Data.Models.DtoModels.ShoppingCartProduct;
    using Palitra27.Data.Models.Enums;
    using Palitra27.Web.ViewModels.FavouriteList;
    using Palitra27.Web.ViewModels.Orders;
    using Palitra27.Web.ViewModels.Products;
    using Palitra27.Web.ViewModels.ShoppingCart;

    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {

            this.CreateMap<Order, CompleteOrderViewModel>();
            this.CreateMap<Order, ConfirmOrderViewModel>()
                           .ForMember(x => x.TotalPrice, y => y.MapFrom(src => src.TotalPrice));
            this.CreateMap<Order, OrderDetailsViewModel>();
            this.CreateMap<Order, IndexUnprocessedОrdersViewModels>();
            this.CreateMap<Order, IndexProcessedOrdersViewModels>();
            this.CreateMap<Order, MyOrderViewModel>();
            this.CreateMap<Order, DeliveredOrdersViewModels>();

            this.CreateMap<FavouriteProduct, FavouriteProductViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
                .ForMember(x => x.Id, y => y.MapFrom(src => src.ProductId))
                .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Product.Price));

            this.CreateMap<OrderDTO, OrderCreateBindingModel>()
                .ForMember(x => x.Country, y => y.MapFrom(src => src.Country.Name))
                .ForMember(x => x.OrderStatus, y => y.MapFrom(src => src.Status));

            this.CreateMap<ApplicationUserDTO, Order>()
              .ForMember(x => x.UserId, y => y.MapFrom(src => src.Id))
              .ForMember(x => x.FirstName, y => y.Ignore())
              .ForMember(x => x.LastName, y => y.Ignore())
              .ForMember(x => x.PhoneNumber, y => y.Ignore());

            this.CreateMap<Country, Order>()
             .ForMember(x => x.CountryId, y => y.MapFrom(src => src.Id))
             .ForMember(x => x.Country, y => y.MapFrom(src => src));

            this.CreateMap<OrderCreateBindingModel, Order>()
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.PhoneNumber))
                .ForMember(x => x.Country, y => y.Ignore())
                .ForMember(x => x.CountryId, y => y.Ignore())
                .ForMember(x => x.PaymentStatus, y => y.MapFrom(src => PaymentStatus.Unpaid))
                .ForMember(x => x.OrderDate, y => y.MapFrom(src => DateTime.UtcNow))
                .ForMember(x => x.Status, y => y.MapFrom(src => OrderStatus.Processed))
                .ForMember(x => x.DeliveryPrice, y => y.MapFrom(src => src.DeliveryPrice))
                .ForMember(x => x.DeliveryDate, y => y.MapFrom(src => DateTime.UtcNow.AddDays(7)))
                .ForMember(x => x.Id, y => y.Ignore());

            this.CreateMap<Brand, BrandDTO>();
            this.CreateMap<BrandDTO, Brand>();

            this.CreateMap<AddReviewBindingModel, Review>()
                .ForMember(x => x.DateOfCreation, y => y.MapFrom(src => DateTime.Now));

            this.CreateMap<Order, OrderDTO>();
            this.CreateMap<OrderDTO, Order>();
            this.CreateMap<OrderProduct, ShoppingCartProductsViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
                .ForMember(x => x.Id, y => y.MapFrom(src => src.Product.Id))
                .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Price))
                .ForMember(x => x.Quantity, y => y.MapFrom(src => src.Quantity))
                .ForMember(x => x.TotalPrice, y => y.MapFrom(src => src.Product.Price));

            this.CreateMap<Order, OrderCreateBindingModel>()
                .ForMember(x => x.Country, y => y.MapFrom(src => src.Country.Name));

            this.CreateMap<Category, CategoryDTO>();
            this.CreateMap<CategoryDTO, Category>();

            this.CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(x => x.Username, y => y.MapFrom(d => d.UserName))
                .ForMember(x => x.UserName, y => y.MapFrom(d => d.UserName))
                .ForMember(x => x.PhoneNumber, y => y.MapFrom(d => d.PhoneNumber));

            this.CreateMap<Review, ReviewDTO>();
            this.CreateMap<ReviewDTO, Review>();

            this.CreateMap<ShoppingCartProductDTO, ShoppingCartProduct>();
            this.CreateMap<ShoppingCartProduct, ShoppingCartProductDTO>();
            this.CreateMap<ShoppingCartProductDTO, ShoppingCartProductsViewModel>()
                .ForMember(x => x.TotalPrice, y => y.MapFrom(src => src.Quantity * src.Product.Price))
                .ForMember(x => x.Price, y => y.MapFrom(src => src.Product.Price))
                .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
                .ForMember(x => x.Id, y => y.MapFrom(src => src.Product.Id))
                .ForMember(x => x.Image, y => y.MapFrom(src => src.Product.Image));

            this.CreateMap<ShoppingCart, ShoppingCartDTO>();
            this.CreateMap<ShoppingCartDTO, ShoppingCart>();

            this.CreateMap<ProductDTO, Product>()
                .ForMember(x => x.Category, y => y.MapFrom(c => c.Category))
                .ForMember(x => x.Brand, y => y.MapFrom(c => c.Brand))
                .ForMember(x => x.Reviews, y => y.MapFrom(c => c.Reviews));
            this.CreateMap<Product, ProductDTO>()
                        .ForMember(x => x.Category, y => y.MapFrom(c => c.Category))
                        .ForMember(x => x.Brand, y => y.MapFrom(c => c.Brand))
                .ForMember(x => x.Reviews, y => y.MapFrom(c => c.Reviews));
            this.CreateMap<ProductDTO, ProductInfoViewModel>()
                .ForMember(x => x.Category, y => y.MapFrom(c => c.Category.Name))
                .ForMember(x => x.Brand, y => y.MapFrom(c => c.Brand.Name));
            this.CreateMap<Product, ShoppingCartProductsViewModel>();
            this.CreateMap<Product, ProductInfoViewModel>()
                .ForMember(x => x.Category, y => y.MapFrom(c => c.Category.Name))
                .ForMember(x => x.Brand, y => y.MapFrom(c => c.Brand.Name));
            this.CreateMap<ProductDTO, ShoppingCartProductsViewModel>();
        }
    }
}
