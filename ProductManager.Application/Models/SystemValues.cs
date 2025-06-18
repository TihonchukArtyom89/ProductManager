using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public class SystemValues
{
    public static Product GetProductNotFound(string searchQuery)
    {
        return new Product()
        {
            ProductID = 0,
            CategoryID = 1,
            ProductName = "Не найдено!",
            ProductDescription = "Продуктов с запросом '" + searchQuery + "' нет!"
        };
    }
    public static Product GetProductNull(Category? category = null)
    {
        return new Product()
        {
            ProductID = 0,
            CategoryID = 1,
            ProductName = "Нет в наличии!",
            ProductDescription = category == null ? "Продуктов данной категории не имеется!" : "Продуктов в категории " + category.CategoryName + " не имеется!"
        };
    }
    public static Pricelist GetPricelistNull()
    {
        return new Pricelist()
        {
            PricelistId = null,
            PricelistName = "Нет прайс-листов!",
            PriceListDateCreation = null,
            PriceListDateModification = null,
        };
    }
    public static Pricelist GetPricelistNotFound(string searchQuery)
    {
        return new Pricelist()
        {
            PricelistId = null,
            PricelistName = "Не найдено таких прайслистов: " + searchQuery + " !",
            PriceListDateCreation = null,
            PriceListDateModification = null,
        };
    }
}
