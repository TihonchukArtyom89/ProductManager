﻿using ProductManager.Application.Models.DBEntities;

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
}
