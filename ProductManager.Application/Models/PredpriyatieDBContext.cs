using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public class PredpriyatieDBContext : DbContext
{
    public PredpriyatieDBContext(DbContextOptions<PredpriyatieDBContext> options) : base(options) 
    {
    }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Pricelist> Pricelists => Set<Pricelist>();
    public DbSet<PricelistOptionalParameter> PricelistOptionalParameters => Set<PricelistOptionalParameter>();
    public DbSet<PricelistProductPurchase> PricelistProductPurchases => Set<PricelistProductPurchase>();
    public DbSet<OptionalParameter> OptionalParameters => Set<OptionalParameter>();
    public DbSet<ProductQuantity> ProductQuantities => Set<ProductQuantity>();
    public DbSet<ProductQuantityType> ProductQuantityTypes => Set<ProductQuantityType>();
}
