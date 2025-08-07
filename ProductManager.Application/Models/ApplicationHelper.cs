using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.Models;

public class ApplicationHelper
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
            PricelistID = null,
            PricelistName = "Нет прайс-листов!",
            PriceListDateCreation = null,
            PriceListDateModification = null,
        };
    }
    public static Pricelist GetPricelistNotFound(string searchQuery)
    {
        return new Pricelist()
        {
            PricelistID = null,
            PricelistName = "Не найдено таких прайслистов: " + searchQuery + " !",
            PriceListDateCreation = null,
            PriceListDateModification = null,
        };
    }
    public static SortOrder SaveSortOrderState(SortOrder sortOrder)
    {

        if (sortOrder == SortOrder.NameAsc || sortOrder == SortOrder.NameDesc)
        {
            sortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        }
        if (sortOrder == SortOrder.PriceAsc || sortOrder == SortOrder.PriceDesc)
        {
            sortOrder = sortOrder == SortOrder.PriceDesc ? SortOrder.PriceAsc : SortOrder.PriceDesc;
        }
        if (sortOrder == SortOrder.DateCreationAsc || sortOrder == SortOrder.DateCreationDesc)
        {
            sortOrder = sortOrder == SortOrder.DateCreationDesc ? SortOrder.DateCreationAsc : SortOrder.DateCreationDesc;
        }
        if (sortOrder == SortOrder.DateModificationAsc || sortOrder == SortOrder.DateModificationDesc)
        {
            sortOrder = sortOrder == SortOrder.DateModificationDesc ? SortOrder.DateModificationAsc : SortOrder.DateModificationDesc;
        }
        return sortOrder;
    }
}
