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
}
