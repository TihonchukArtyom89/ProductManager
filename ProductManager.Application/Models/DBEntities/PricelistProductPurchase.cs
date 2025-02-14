using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной покупке товара в прайслисте за определённую цену и по указанному количеству
[Table("PricelistProductPurchases")]
public class PricelistProductPurchase
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PurchaseID { get; set; }//ИД покупки товара//уникальный,первичный ключ
    public long PricelistID { get; set; }//ИД прайслиста, куда была занесена покупка товара// внешний ключ на таблицу прайслистов
    public long ProductID { get; set; }//ИД товара, который был куплен// внешний ключ на таблицу товаров
    public float ProductQuantity { get; set; }//количество товаров(не целочисленная так как товар может быть измеряться по весу)
    public decimal ProductPrice { get; set; }//цена по которой был куплен товар(именно весь товара не единично)
}
