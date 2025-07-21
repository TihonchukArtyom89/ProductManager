using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной покупке продукта в прайслисте за определённую цену и по указанному количеству
[Table("PricelistProductPurchases")]
public class PricelistProductPurchase
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? PurchaseID { get; set; }//ИД покупки продукта//уникальный,первичный ключ
    public long? PricelistID { get; set; }//ИД прайслиста, куда была занесена покупка продукта// внешний ключ на таблицу прайслистов
    public Pricelist? Pricelist { get; set; }//навигационное св-во на таблицу прайслистов покупок продуктов(на главную сущность)
    public long? ProductID { get; set; }//ИД продукта, который был куплен// внешний ключ на таблицу продуктов
    public Product? Product { get; set; }//навигационное св-во на таблицу продуктов(на главную сущность)
    [DisplayName("Количество продуктов в покупке")]
    [Required(ErrorMessage = "Введите количество продуктов в прайслисте.")]
    public double ProductQuantityNumber { get; set; }//количество продуктов, которое было приобретено в данной покупке (не целочисленная так как продукт может быть измеряться не поштучно - может вводится черз форму заполнения прайслиста)
    [DisplayName("Цена продукта")]
    [Required(ErrorMessage = "Укажите цену купленного продукта.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше ноля.")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal ProductPriceAtBuy { get; set; }//цена по которой был куплен продукт(именно весь продукт, а не единично)
    public string ProductNameAtBuy { get; set; } = string.Empty;//наименование продукта во время покупки
    public string ProductQuantityNameAtBuy { get; set; } = string.Empty;//наименование количества продукта (например: шт., кг., л.) во время покупки продукта
    public List<PricelistOptionalParameter>? PricelistOptionalParameters { get; set; }//навигационное св-во на таблицу опциональных параметров покупки в прайслисте (на зависимую сущность)
}
