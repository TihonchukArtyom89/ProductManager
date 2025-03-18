using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ProductManager.Application.Models.DBEntities;
using System.Collections.Generic;

namespace ProductManager.Application.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        try
        {
            PredpriyatieDBContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PredpriyatieDBContext>();
            if (!context.Database.CanConnect())//check if app can connect to database
            {
                //throw new Exception("can not connect to database( may be phisycal file of db is not existing)");
                //create empty database from dbcontext - ??? 
                //ensure methods and migrate not working
            }
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Products.Any())
            {
                if (!context.Categories.Any())
                {//code for insert sample data to table Categories(categories of product) 
                    context.Categories.AddRange(//fill Categories Table with sample data
                        new Category { CategoryName = new Category().CategoryName },
                        new Category { CategoryName = "Мебель" },
                        new Category { CategoryName = "Фрукты" },
                        new Category { CategoryName = "test" });
                    context.SaveChanges();
                }
                Category category_1 = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()!;//add value for null categories
                Category category_2 = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()!;//add value for null categories
                context.Products.AddRange(//fill Products Table with sample data
                    new Product { ProductName = "Стул", ProductDescription = "Обычный стул", ProductPrice = 1547.04m, CategoryID = category_1.CategoryID },
                    new Product { ProductName = "Яблоко", ProductDescription = "Красное, наливное", ProductPrice = 196.67m, CategoryID = category_2.CategoryID},
                    new Product { ProductName = "Слива", ProductDescription = "Спелая,садовая", ProductPrice = 378.00m, CategoryID = category_2.CategoryID },
                    new Product { ProductName = "Стол № 1", ProductDescription = "Для обеда в саду", ProductPrice = 3098.39m, CategoryID = category_1.CategoryID },
                    new Product { ProductName = "Груша", ProductDescription = "Можно скушать", ProductPrice = 247.07m, CategoryID = category_2.CategoryID },
                    new Product { ProductName = "Стол  № 2", ProductDescription = "Компьтерный стол", ProductPrice = 15999.98m, CategoryID = category_1.CategoryID },
                    new Product { ProductName = "Табуретка", ProductDescription = "Стильная, модная", ProductPrice = 6999.98m, CategoryID = category_1.CategoryID },
                    new Product { ProductName = "Маракуйя", ProductDescription = "Фрукт страсти", ProductPrice = 2399.07m, CategoryID = category_2.CategoryID }
                    );
                context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            WriteLine("Exception message:\n" + ex.Message.ToString() + "Inner exception message:\n" + ex.InnerException?.Message);
        }
    }
}