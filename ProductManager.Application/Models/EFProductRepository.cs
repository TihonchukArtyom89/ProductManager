using ProductManager.Application.Models.DBEntities;
using System.Text.Json;

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
    public IQueryable<DeletedRecord> DeletedRecords => context.DeletedRecords;
    public void CreateProduct(Product p)
    {
        context.Add(p);
        context.SaveChanges();
    }
    public void UpdateProduct(Product p)
    {
        context.Update(p);
        context.SaveChanges();
    }
    public void DeleteProduct(Product p)
    {
        DeletedRecord deletedRecord = new DeletedRecord()
        {//make record of deleted product to table of deleted recoreds
            RecordDeleteDateTime = DateTime.Now,
            TableSourceDeletedRecordID = p.ProductID,
            TableSourceName = "Products",// may be it can get from class Product somehow?
            TableSourceDeletedRecordValueFromJSON = JsonSerializer.Serialize(context.Products.Where(e => e.ProductID == p.ProductID).FirstOrDefault()).ToString()
        };
        context.Add(deletedRecord);//add record of deleted product to table of deleted records
        foreach (PricelistProductPurchase purchase in context.PricelistProductPurchases)
        {//make null id of deleted product in product purchases in pricelist
            if (purchase.ProductID == p.ProductID)
            {
                purchase.ProductID = null;
                //purchase.IsProductDelete = true;
            }
        }
        context.Products.Remove(context.Products.Where(e=>e.ProductID == p.ProductID).FirstOrDefault() ?? new Product());
        context.SaveChanges();
    }
}
