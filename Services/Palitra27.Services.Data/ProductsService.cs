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

        public IQueryable<Product> Brands()
        {
            return this.context.Products;
        }

        public IQueryable<Product> Categories()
        {
            return this.context.Products;
        }

        public void Create(CreateProductBindingModel model)
        {
            var category = this.context.Categories.FirstOrDefault(c => c.Name == model.Category);
            Product product = new Product() { Category = category, Image = model.Image, Price = model.Price, Name = model.ProductName };
            this.context.Products.Add(product);
            this.context.SaveChanges();
        }
    }
}
