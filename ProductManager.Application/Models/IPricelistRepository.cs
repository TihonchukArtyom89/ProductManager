using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public interface IPricelistRepository 
{
    IQueryable<Pricelist> Pricelists { get; }
    IQueryable<PricelistOptionalParameter> PricelistOptionalParameters { get; }
    IQueryable<PricelistProductPurchase> PricelistProductPurchases { get; }
    IQueryable<ProductQuantity> ProductQuantities { get; }
    IQueryable<ProductQuantityType> ProductQuantityTypes { get; }
    IQueryable<OptionalParameter> OptionalParameters { get; }
    void CreatePriceList(Pricelist pricelist);
    void UpdatePriceList(Pricelist pricelist);
    void DeletePriceList(Pricelist pricelist);
    void CreatePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter);
    void UpdatePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter);
    void DeletePricelistOptionalParameter(PricelistOptionalParameter pricelistOptionalParameter);
    void CreatePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase);
    void UpdatePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase);
    void DeletePricelistProductPurchase(PricelistProductPurchase pricelistProductPurchase);
    void CreateProductQuantity(ProductQuantity productQuantity);
    void UpdateProductQuantity(ProductQuantity productQuantity);
    void DeleteProductQuantity(ProductQuantity productQuantity);
    void CreateProductQuantityType(ProductQuantityType productQuantityType);
    void UpdateProductQuantityType(ProductQuantityType productQuantityType);
    void DeleteProductQuantityType(ProductQuantityType productQuantityType);
    void CreateOptionalParameter(OptionalParameter optionalParameter);
    void UpdateOptionalParameter(OptionalParameter optionalParameter);
    void DeleteOptionalParameter(OptionalParameter optionalParameter);
}
