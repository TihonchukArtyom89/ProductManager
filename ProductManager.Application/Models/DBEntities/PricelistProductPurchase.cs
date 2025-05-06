using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной покупке продукта в прайслисте за определённую цену и по указанному количеству
[Table("PricelistProductPurchases")]
public class PricelistProductPurchase
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long PurchaseID { get; set; }//ИД покупки продукта//уникальный,первичный ключ
    public long PricelistID { get; set; }//ИД прайслиста, куда была занесена покупка продукта// внешний ключ на таблицу прайслистов
    public long ProductID { get; set; }//ИД продукта, который был куплен// внешний ключ на таблицу продуктов
    [DisplayName("Количество продуктов в покупке")]
    [Required(ErrorMessage = "Введите количество продуктов в прайслисте.")]
    public float ProductQuantityNumber { get; set; }//количество продуктов(не целочисленная так как продукт может быть измеряться по весу - может вводится черз форму)
    [DisplayName("Цена продукта")]
    [Required(ErrorMessage = "Укажите цену купленного продукта.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше ноля.")]
    [Column(TypeName = "decimal(8,2)")]
    public decimal ProductPrice { get; set; }//цена по которой был куплен продукт(именно весь продукт, а не единично)
    public Product? Product { get; set; }//навигационное св-во на таблицу продуктов(на главную сущность)
    public Pricelist? Pricelist { get; set; }//навигационное св-во на таблицу прайслистов покупок продуктов(на главную сущность)
    public ProductQuantity? ProductQuantity { get; set; }//навигационное св-во на таблицу разновидностей количеств продукта(на главную сущность)
}
