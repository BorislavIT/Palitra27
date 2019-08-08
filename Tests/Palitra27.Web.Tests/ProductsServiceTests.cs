namespace Palitra27.Web.Tests
{
    using System;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Data.Models.DtoModels.Brand;
    using Palitra27.Data.Models.DtoModels.Category;
    using Palitra27.Data.Models.DtoModels.Product;
    using Palitra27.Services.Data;
    using Palitra27.Web.MappingConfigurations;
    using Palitra27.Web.ViewModels.Brands;
    using Palitra27.Web.ViewModels.Categories;
    using Palitra27.Web.ViewModels.Products;
    using Xunit;

    public class ProductsServiceTests
    {
        [Fact]
        public void CreateProductShouldCreateProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateProductShouldCreateProduct_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);
            var products = dbContext.Products.ToList();

            Assert.True(products.FirstOrDefault(x => x.Name == productDTO.Name) != null);
        }

        [Fact]
        public void CreateProductShouldntCreateProductIfInvalidBrandOrCategory()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateProductShouldntCreateProductIfInvalidBrandOrCategory_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = mapper.Map<BrandDTO>(new Brand { Name = "InvalidName" });
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            Assert.Null(productDTO);
        }

        [Fact]
        public void CreateProductShouldntCreateProductIfAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"CreateProductShouldntCreateProductIfAlreadyExists_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = mapper.Map<BrandDTO>(new Brand { Name = "InvalidName" });
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            Assert.Null(productDTO);
        }

        [Fact]
        public void FindProductByIdShouldReturnProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindProductByIdShouldReturnProduct_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);

            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var productFromService = productService.FindProductById(productDTO.Id);

            Assert.Equal(productDTO.Name, productFromService.Name);
        }

        [Fact]
        public void FindDomainProductShouldReturnDomainProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindDomainProductShouldReturnDomainProduct_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);

            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var product = productService.FindDomainProduct(productDTO.Id);

            Assert.IsType<Product>(product);
        }

        [Fact]
        public void FindProductByIdShouldReturnNullIfProductDoesntExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindProductByIdShouldReturnNullIfProductDoesntExist_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);

            var invalidGuidId = productService.FindProductById(Guid.NewGuid().ToString());

            Assert.Null(invalidGuidId);
        }

        [Fact]
        public void EditProductMainShouldReturnNullIfProductDoesntExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                       .UseInMemoryDatabase(databaseName: $"EditProductShouldEditMainProductParams_Product_Database")
                       .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.FindProductById("asd");

            var editedProduct = productService.EditProduct(new ProductEditBindingModel { });

            Assert.Null(editedProduct);
        }

        [Fact]
        public void FindProductByIdShoulReturnProduct()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindProductByIdShoulReturnProduct_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var productFromService = productService.FindProductById(productDTO.Id);

            Assert.Equal(productName, productFromService.Name);
        }

        [Fact]
        public void EditProductShouldEditMainProductParams()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditProductShouldEditMainProductParams_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var editedProduct = productService.EditProduct(this.CreateProductEditBindingModel(productDTO, mapper));

            var productFromService = productService.FindProductById(productDTO.Id);

            Assert.NotEqual(editedProduct, productDTO);
        }

        [Fact]
        public void EditDescriptionShouldEditDescription()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditDescriptionShouldEditDescription_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var editedProduct = productService.EditDescription(this.CreateProductEditDescriptionBindingModel(productDTO));

            var productFromService = productService.FindProductById(productDTO.Id);

            Assert.NotEqual(editedProduct, productDTO);
        }

        [Fact]
        public void EditSpecificationsShouldEditSpecifications()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"EditSpecificationsShouldEditSpecifications_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var editedProduct = productService.EditSpecifications(this.CreateEditSpecificationsBindingModel(productDTO));

            var productFromService = productService.FindProductById(productDTO.Id);

            Assert.NotEqual(editedProduct, productDTO);
        }

        [Fact]
        public void AddReviewShouldAddReview()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"AddReviewShouldAddReview_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);
            var usersService = new UsersService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var reviewBindingModel = new AddReviewBindingModel { Message = "Blabla", Name = "yaah", Stars = 5 };

            var userToAdd = new ApplicationUser { UserName = "LegitUsername" };

            dbContext.Users.Add(userToAdd);
            dbContext.SaveChanges();

            var user = usersService.FindUserByUsername(userToAdd.UserName);

            var reviewsCountPreAdd = dbContext.Reviews.Count();

            var reviewDTO = productService.AddReview(reviewBindingModel, user);

            var reviewsCountAfterAdd = dbContext.Reviews.Count();

            Assert.NotEqual(reviewsCountPreAdd, reviewsCountAfterAdd);
        }

        [Fact]
        public void FindAllProductsShouldReturnAllProducts()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindAllProductsShouldReturnAllProducts_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);
            var brandService = new BrandsService(dbContext, mapper);
            var categoriesService = new CategoriesService(dbContext, mapper);

            var brand = this.CreateBrandDTO(brandService);
            var category = this.CreateCategoryDTO(categoriesService);

            var productName = Guid.NewGuid().ToString().Substring(0, 15);

            var productBindingModel = this.CreateProductBindingModelByName(brand, category, productName);

            var image = new Mock<IFormFile>();

            var productDTO = productService.Create(productBindingModel, image.Object);

            var allProducts = productService.FindAllProducts();

            Assert.True(allProducts.FirstOrDefault(x => x.Id == productDTO.Id) != null);
        }

        [Theory]
        [InlineData("adsasdgdagdagadasd")]
        [InlineData("asdasgdagdagaddasdg")]
        public void FindProductByIdShouldReturnNullIfInvalidId(string id)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseInMemoryDatabase(databaseName: $"FindProductByIdShouldReturnNullIfInvalidId_Product_Database")
                        .Options;

            var dbContext = new ApplicationDbContext(options);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApplicationProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var productService = new ProductsService(dbContext, mapper);

            var productFromService = productService.FindProductById(id);

            Assert.Null(productFromService);
        }

        private CategoryDTO CreateCategoryDTO(CategoriesService categoriesService)
        {
            return categoriesService.CreateCategory(new CreateCategoryBindingModel { Name = Guid.NewGuid().ToString().Substring(0, 15) });
        }

        private BrandDTO CreateBrandDTO(BrandsService brandsService)
        {
            return brandsService.CreateBrand(new CreateBrandBindingModel { Name = Guid.NewGuid().ToString().Substring(0, 15) });
        }

        private CreateProductBindingModel CreateProductBindingModelByName(BrandDTO brand, CategoryDTO category, string name)
        {
            return new CreateProductBindingModel
            {
                Category = category.Name ?? null,
                Brand = brand.Name ?? null,
                Price = "23",
                Name = name,
            };
        }

        private ProductEditBindingModel CreateProductEditBindingModel(ProductDTO product, IMapper mapper)
        {
            return new ProductEditBindingModel { Id = product.Id, Name = Guid.NewGuid().ToString().Substring(0, 15), Brand = product.Brand.Name ?? null, Category = product.Category.Name ?? null, MiniDescription = product.MiniDescription, Price = product.Price.ToString() };
        }

        private EditDescriptionBindingModel CreateProductEditDescriptionBindingModel(ProductDTO product)
        {
            return new EditDescriptionBindingModel { Description = "LegitDescription", Id = product.Id };
        }

        private EditSpecificationsBindingModel CreateEditSpecificationsBindingModel(ProductDTO product)
        {
            return new EditSpecificationsBindingModel { Width = 0.15M, Id = product.Id, Depth = 0.14M, Height = product.Height, Weight = product.Weight };
        }

    }
}
