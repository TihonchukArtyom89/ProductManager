namespace ProductManager.Application.Models;

public enum SortOrder
{
    Neutral,//сортировка по умолчанию (по ИД - возрастающая)
    NameAsc,//От А до Я
    NameDesc,//От Я до А
    PriceAsc,//от дешёвых продуктов к дорогим - по возрастанию цены продукта
    PriceDesc//от дорогих продуктов к дешёвым - по убыванию цены продукта
}
