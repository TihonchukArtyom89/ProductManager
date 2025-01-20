using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public class EFProductRepository : IProductRepository
{
    private PredpriyatieDBContext context;
    public EFProductRepository(PredpriyatieDBContext ctx)
    {
        context = ctx;
    }
    public IQueryable<Product> Products => context.Products;
    public IQueryable<Category> Categories => context.Categories;
    //public void CreateProduct(Product p)
    //{
    //    context.Add(p);
    //    context.SaveChanges();
    //}
    //public void DeleteProduct(Product p)
    //{
    //    context.Remove(p);
    //    context.SaveChanges();
    //}
    //public void SaveProduct(Product p)
    //{
    //    context.SaveChanges();
    //}
}
