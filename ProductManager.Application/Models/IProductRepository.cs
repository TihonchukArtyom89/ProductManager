using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public interface IProductRepository
{
    IQueryable<Product> Products { get; }
    IQueryable<Category> Categories { get; }
    void CreateProduct(Product p);
    void UpdateProduct(Product p);
    void DeleteProduct(Product p);
}
