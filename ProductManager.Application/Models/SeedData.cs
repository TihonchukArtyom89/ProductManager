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
            if (!context.ProductQuantityTypes.Any())
            {//code for insert sample data to table ProductQuantityTypes
                context.ProductQuantityTypes.AddRange(//fill ProductQuantityTypes Table with sample data
                    new ProductQuantityType { ProductQuantityTypeName = "Поштучно." },
                    new ProductQuantityType { ProductQuantityTypeName = "По весу." },
                    new ProductQuantityType { ProductQuantityTypeName = "По объёму." });
                context.SaveChanges();
                if (!context.ProductQuantities.Any())
                {//code for insert sample data to table ProductQuantity
                    context.ProductQuantities.AddRange(//fill ProductQuantity Table with sample data
                        new ProductQuantity { ProductQuantityTypeID = 1, ProductQuantityName = "шт." },
                        new ProductQuantity { ProductQuantityTypeID = 2, ProductQuantityName = "кг." },
                        new ProductQuantity { ProductQuantityTypeID = 3, ProductQuantityName = "л." });
                    context.SaveChanges();
                }
            }
            if (!context.Products.Any())
            {
                if (!context.Categories.Any())
                {//code for insert sample data to table Categories(categories of product) 
                    context.Categories.AddRange(//fill Categories Table with sample data
                        new Category { CategoryName = new Category().CategoryName },
                        new Category { CategoryName = "Мебель" },
                        new Category { CategoryName = "Фрукты" },
                        new Category { CategoryName = "Напитки" },
                        new Category { CategoryName = "test" });
                    context.SaveChanges();
                }
                Category category_1 = context.Categories.Where(c => c.CategoryName == "Мебель").FirstOrDefault()!;//add value for null categories
                Category category_2 = context.Categories.Where(c => c.CategoryName == "Фрукты").FirstOrDefault()!;//add value for null categories
                Category category_3 = context.Categories.Where(c => c.CategoryName == "Напитки").FirstOrDefault()!;//add value for null categories
                context.Products.AddRange(//fill Products Table with sample data
                    new Product { ProductName = "Стул", ProductDescription = "Обычный стул", ProductPrice = 1547.04m, CategoryID = category_1.CategoryID, ProductQuantityID = 1 },
                    new Product { ProductName = "Яблоко", ProductDescription = "Красное, наливное", ProductPrice = 196.67m, CategoryID = category_2.CategoryID, ProductQuantityID = 2 },
                    new Product { ProductName = "Слива", ProductDescription = "Спелая,садовая", ProductPrice = 378.00m, CategoryID = category_2.CategoryID, ProductQuantityID = 2 },
                    new Product { ProductName = "Стол № 1", ProductDescription = "Для обеда в саду", ProductPrice = 3098.39m, CategoryID = category_1.CategoryID, ProductQuantityID = 1 },
                    new Product { ProductName = "Груша", ProductDescription = "Можно скушать", ProductPrice = 247.07m, CategoryID = category_2.CategoryID, ProductQuantityID = 2 },
                    new Product { ProductName = "Стол  № 2", ProductDescription = "Компьтерный стол", ProductPrice = 15999.98m, CategoryID = category_1.CategoryID, ProductQuantityID = 1 },
                    new Product { ProductName = "Табуретка", ProductDescription = "Стильная, модная", ProductPrice = 6999.98m, CategoryID = category_1.CategoryID, ProductQuantityID = 1 },
                    new Product { ProductName = "Маракуйя", ProductDescription = "Фрукт страсти", ProductPrice = 2399.07m, CategoryID = category_2.CategoryID, ProductQuantityID = 2 },
                    new Product { ProductName = "Молоко", ProductDescription = "Коровье, деревенское, парное", ProductPrice = 799.77m, CategoryID = category_3.CategoryID, ProductQuantityID = 3 },
                    new Product { ProductName = "Вино", ProductDescription = "Виноградное, домашнее", ProductPrice = 1298.65m, CategoryID = category_3.CategoryID, ProductQuantityID = 3 },
                    new Product { ProductName = "Вода", ProductDescription = "Артезианская, ключевая", ProductPrice = 98.65m, CategoryID = category_3.CategoryID, ProductQuantityID = 3 }
                    );
                context.SaveChanges();
                if (!context.Pricelists.Any())
                {
                    context.Pricelists.AddRange(//fill Pricelists Table with sample data
                        new Pricelist
                        {
                            PricelistName = "Январьский прайслист",
                            PriceListDateCreation = new DateTime(2025, 1, 23, 12, 34, 55),
                            PriceListDateModification = new DateTime(2025, 2, 28, 17, 56, 12),
                        },
                        new Pricelist
                        {
                            PricelistName = "Февральский прайслист",
                            PriceListDateCreation = new DateTime(2025, 1, 26, 9, 45, 5),
                            PriceListDateModification = new DateTime(2025, 5, 12, 10, 13, 17),
                        });
                    context.SaveChanges();
                    if (!context.PricelistProductPurchases.Any())
                    {
                        context.PricelistProductPurchases.AddRange(
                            new PricelistProductPurchase
                            {
                                PricelistID = 1,
                                ProductID = 1,
                                ProductNameAtBuy = "Стул",
                                ProductPriceAtBuy = 1547.04m,
                                ProductQuantityNameAtBuy = "шт.",
                                ProductQuantityNumber = 2
                            },
                            new PricelistProductPurchase
                            {
                                PricelistID = 1,
                                ProductID = 2,
                                ProductNameAtBuy = "Яблоко",
                                ProductPriceAtBuy = 196.67m,
                                ProductQuantityNameAtBuy = "кг.",
                                ProductQuantityNumber = 3.47
                            },
                            new PricelistProductPurchase
                            {
                                PricelistID = 1,
                                ProductID = 3,
                                ProductNameAtBuy = "Слива",
                                ProductPriceAtBuy = 378.00m,
                                ProductQuantityNameAtBuy = "кг.",
                                ProductQuantityNumber = 2.31
                            },
                            new PricelistProductPurchase
                            {
                                PricelistID = 2,
                                ProductID = 5,
                                ProductNameAtBuy = "Груша",
                                ProductPriceAtBuy = 247.07m,
                                ProductQuantityNameAtBuy = "кг.",
                                ProductQuantityNumber = 1.98
                            },
                            new PricelistProductPurchase
                            {
                                PricelistID = 2,
                                ProductID = 6,
                                ProductNameAtBuy = "Стол  № 2",
                                ProductPriceAtBuy = 15999.98m,
                                ProductQuantityNameAtBuy = "шт.",
                                ProductQuantityNumber = 1
                            },
                            new PricelistProductPurchase
                            {
                                PricelistID = 2,
                                ProductID = 9,
                                ProductNameAtBuy = "Вода",
                                ProductPriceAtBuy = 98.65m,
                                ProductQuantityNameAtBuy = "л.",
                                ProductQuantityNumber = 1.5
                            }
                            );
                        context.SaveChanges();
                        if (!context.OptionalParameters.Any())
                        {
                            context.OptionalParameters.AddRange(
                                new OptionalParameter //январьский прайслист
                                {
                                    OptionalParameterName = "Дополнительные опции"
                                },
                                new OptionalParameter
                                {
                                    OptionalParameterName = "Доставка"
                                },
                                new OptionalParameter //февральский прайслист
                                {
                                    OptionalParameterName = "Особенности"
                                },
                                new OptionalParameter
                                {
                                    OptionalParameterName = "Акция"
                                },
                                new OptionalParameter
                                {
                                    OptionalParameterName = "Складские метки"
                                }
                                );
                            context.SaveChanges();
                            if (!context.PricelistOptionalParameters.Any())
                            {
                                context.PricelistOptionalParameters.AddRange(
                                    //январьский прайслист
                                    //Стул кол-во 2
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 1, //Дополнительные опции
                                        OptionalParameterValue = "Сборка на месте",
                                        PurchaseID = 1,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 2, //Доставка
                                        OptionalParameterValue = "Доставка к двери",
                                        PurchaseID = 1,
                                    },
                                    //Яблоко кол-во 3.47
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 1,//Дополнительные опции
                                        OptionalParameterValue = "Экологичная упаковка",
                                        PurchaseID = 2,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 2, //Доставка
                                        OptionalParameterValue = "Курьер",
                                        PurchaseID = 2,
                                    },
                                    //Слива кол-во 2.31
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 1,//Дополнительные опции
                                        OptionalParameterValue = "Подарочное оформление",
                                        PurchaseID = 3,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 2,//Доставка
                                        OptionalParameterValue = "Самовывоз",
                                        PurchaseID = 3,
                                    },
                                    //февральский прайслист
                                    //Груша кол-во 1.98
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 3,//Особенности
                                        OptionalParameterValue = "Желтая, крупная, египетская",
                                        PurchaseID = 4,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 4,//Акция
                                        OptionalParameterValue = " - ",
                                        PurchaseID = 4,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 5,//Складские метки
                                        OptionalParameterValue = "Вегетерианская",
                                        PurchaseID = 4,
                                    },
                                    //Стол № 2 кол-во 1
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 3,//Особенности
                                        OptionalParameterValue = "Остаток с прошлого года",
                                        PurchaseID = 5,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 4,//Скидки в феврале
                                        OptionalParameterValue = "В подарок бонусная карта",
                                        PurchaseID = 5,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 5,//Складские метки
                                        OptionalParameterValue = "Уценка",
                                        PurchaseID = 5,
                                    },
                                    //Вода кол-во 1.5
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 3,//Особенности
                                        OptionalParameterValue = "Остатки с ликвидированного филиала",
                                        PurchaseID = 6,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 4,//Акция
                                        OptionalParameterValue = "Пять бутылок по цене четырёх",
                                        PurchaseID = 6,
                                    },
                                    new PricelistOptionalParameter
                                    {
                                        OptionalParameterID = 5,//Складские метки
                                        OptionalParameterValue = "Без газа",
                                        PurchaseID = 6,
                                    }
                                    );
                                context.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLine("Exception message:\n" + ex.Message.ToString() + "Inner exception message:\n" + ex.InnerException?.Message);
        }
    }
}