using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой продуктам
[Table("Products")]
public class Product
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ProductID { get; set; }//ИД продукта//уникальный,первичный ключ
    [DisplayName("Название продукта")]
    [Required(ErrorMessage = "Name of product is required.")]
    public string ProductName { get; set; } = string.Empty;//название продукта
    [DisplayName("Описание продукта")]
    [Required(ErrorMessage = "Description of product is required.")]
    public string ProductDescription { get; set; } = string.Empty;//описание продукта
    [Required(ErrorMessage = "Category of product is required.")]
    public long? CategoryID { get; set; }//ИД категория продукта// внешний ключ на таблицу категорий продукта
    public Category? Category { get; set; }//навигационное св-во на таблицу категорий(на главную сущность)
    [DisplayName("Цена продукта")]
    [Required(ErrorMessage = "Price of product is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Enter a positive price")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "decimal(8,2)")]
    public decimal ProductPrice { get; set; }//цена продукта
    public List<PricelistProductPurchase>? PricelistProductPurchases { get; set; }//навигационное св-во на таблицу покупок(на зависимую сущность)
}
