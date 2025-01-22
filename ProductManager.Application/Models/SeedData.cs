using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Models.DBEntities;
using System.Collections.Generic;

namespace ProductManager.Application.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        PredpriyatieDBContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PredpriyatieDBContext>();
        try
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Products.Any())
            {
                string[] CategoryNames = { "Нет категории","Мебель", "Фрукты", "test" };
                if (!context.Categories.Any())
                {//code for insert sample data to table Categories(categories of product) 
                    context.Categories.AddRange(new Category { CategoryName = CategoryNames[0] }, new Category { CategoryName = CategoryNames[1] }, new Category { CategoryName = CategoryNames[2] }, new Category { CategoryName = CategoryNames[3] });
                    context.SaveChanges();
                }              
                context.Products.AddRange(//fill Products Table with sample data
                    new Product { ProductName = "Стул", ProductDescription = "Обычный стул", ProductPrice = 1547.04m, CategoryID = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Яблоко", ProductDescription = "Красное, наливное", ProductPrice = 196.67m, CategoryID = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Слива", ProductDescription = "Спелая,садовая", ProductPrice = 378.00m, CategoryID = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Стол № 1", ProductDescription = "Для обеда в саду", ProductPrice = 3098.39m, CategoryID = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Груша", ProductDescription = "Можно скушать", ProductPrice = 247.07m, CategoryID = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Стол  № 2", ProductDescription = "Компьтерный стол", ProductPrice = 15999.98m, CategoryID = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Табуретка", ProductDescription = "Стильная, модная", ProductPrice = 6999.98m, CategoryID = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()?.CategoryID },
                    new Product { ProductName = "Маракуйя", ProductDescription = "Фрукт страсти", ProductPrice = 2399.07m, CategoryID = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()?.CategoryID }
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