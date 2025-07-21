namespace ProductManager.Application.Models.DBEntities;

public class EFPricelistRepository:IPricelistRepository
{
    private readonly PredpriyatieDBContext context;
    public EFPricelistRepository(PredpriyatieDBContext ctx)
    {
        context = ctx;
    }
    public IQueryable<Pricelist> Pricelists => context.Pricelists;
    public IQueryable<ProductQuantity> ProductQuantities => context.ProductQuantities;
    public IQueryable<ProductQuantityType> ProductQuantityTypes => context.ProductQuantityTypes;
    public IQueryable<OptionalParameter> OptionalParameters => context.OptionalParameters;
    public IQueryable<PricelistOptionalParameter> PricelistOptionalParameters => context.PricelistOptionalParameters;
    public IQueryable<PricelistProductPurchase> PricelistProductPurchases => context.PricelistProductPurchases;
    public IQueryable<DeletedRecord> DeletedRecords => context.DeletedRecords;
    public IQueryable<Product> Products => context.Products;
    public IQueryable<Category> Categories => context.Categories;  
    public void CreatePriceList(Pricelist pricelist)
    {
        context.Add(pricelist);
        context.SaveChanges();
    }
    public void UpdatePriceList(Pricelist pricelist)
    {
        context.Update(pricelist);
        context.SaveChanges();
    }
    public void DeletePriceList(Pricelist pricelist)
    {
        context.Remove(pricelist);
        context.SaveChanges();
    }
    public void CreatePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter)
    {
        context.Add(pricelistOptionalParameter);
        context.SaveChanges();
    }
    public void UpdatePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter)
    {
        context.Update(pricelistOptionalParameter);
        context.SaveChanges();
    }
    public void DeletePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter)
    {
        context.Remove(pricelistOptionalParameter);
        context.SaveChanges();
    }
    public void CreatePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase)
    {
        context.Add(pricelistProductPurchase);
        context.SaveChanges();
    }
    public void UpdatePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase)
    {
        context.Update(pricelistProductPurchase);
        context.SaveChanges();
    }
    public void DeletePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase)
    {
        context.Remove(pricelistProductPurchase);
        context.SaveChanges();
    }
    public void CreateProductQuantity(ProductQuantity productQuantity)
    {
        context.Add(productQuantity);
        context.SaveChanges();
    }
    public void UpdateProductQuantity(ProductQuantity productQuantity)
    {
        context.Update(productQuantity);
        context.SaveChanges();
    }
    public void DeleteProductQuantity(ProductQuantity productQuantity)
    {
        context.Remove(productQuantity);
        context.SaveChanges();
    }   
    public void CreateProductQuantityType(ProductQuantityType productQuantityType)
    {
        context.Add(productQuantityType);
        context.SaveChanges();
    }   
    public void UpdateProductQuantityType(ProductQuantityType productQuantityType)
    {
        context.Update(productQuantityType);
        context.SaveChanges();
    }
    public void DeleteProductQuantityType(ProductQuantityType productQuantityType)
    {
        context.Remove(productQuantityType);
        context.SaveChanges();
    }
    public void CreateOptionalParameter(OptionalParameter optionalParameter)
    {
        context.Add(optionalParameter);
        context.SaveChanges();
    }
    public void UpdateOptionalParameter(OptionalParameter optionalParameter)
    {
        context.Update(optionalParameter);
        context.SaveChanges();
    }
    public void DeleteOptionalParameter(OptionalParameter optionalParameter)
    {
        context.Remove(optionalParameter);
        context.SaveChanges();
    }
}
