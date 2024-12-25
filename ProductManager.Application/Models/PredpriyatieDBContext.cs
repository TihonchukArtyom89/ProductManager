using Microsoft.EntityFrameworkCore;

namespace ProductManager.Application.Models;

public class PredpriyatieDBContext : DbContext
{
    public PredpriyatieDBContext(DbContextOptions<PredpriyatieDBContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PriceList> PriceLists => Set<PriceList>();
    public DbSet<PriceListProduct> PriceListProducts => Set<PriceListProduct>();
    public DbSet<OptionalParameter> OptionalParameters => Set<OptionalParameter>();
    public DbSet<PriceListOptionalParameter> PriceListOptionalParameters => Set<PriceListOptionalParameter>();
}
