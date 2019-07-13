namespace Palitra27.Services.Data
{
    using System.Linq;

    using Palitra27.Data;
    using Palitra27.Data.Models;
    using Palitra27.Web.ViewModels.Products;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext context;

        public ProductsService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<ProductBrand> FindAllBrands()
        {
            var brands = this.context.ProductsBrands;

            return brands;
        }

        public IQueryable<Category> FindAllCategories()
        {
            var categories = this.context.Categories;

            return categories;
        }

        public void Create(CreateProductBindingModel model)
        {
            var brand = this.context.ProductsBrands.FirstOrDefault(c => c.Name == model.Brand);
            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            Product product = new Product() { Category = category, Image = model.Image, Price = model.Price, Name = model.ProductName, Brand = brand };
            this.context.Products.Add(product);
            this.context.SaveChanges();
        }

        public Product FindProductById(string id)
        {
            var product = this.context.Products.FirstOrDefault(p => p.Id == id);
            return product;
        }
    }
}
