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
    [Required(ErrorMessage = "Введите название продукта.")]
    [StringLength(50)]
    public string ProductName { get; set; } = string.Empty;//название продукта
    [DisplayName("Описание продукта")]
    [Required(ErrorMessage = "Введите описание продукта.")]
    [StringLength(300)]
    public string ProductDescription { get; set; } = string.Empty;//описание продукта
    [Required(ErrorMessage = "Выберите категорию продукта.")]
    public long? CategoryID { get; set; }//ИД категория продукта// внешний ключ на таблицу категорий продукта
    public Category? Category { get; set; }//навигационное св-во на таблицу категорий(на главную сущность)
    [DisplayName("Цена продукта")]
    [Required(ErrorMessage = "Введите цену продукта.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше ноля.")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(8,2)")]
    public decimal ProductPrice { get; set; }//цена продукта
    public List<PricelistProductPurchase>? PricelistProductPurchases { get; set; }//навигационное св-во на таблицу покупок(на зависимую сущность)
    public Product() { CategoryID = new Category().CategoryID; }
    public ProductQuantity? ProductQuantity { get; set; }//навигационное св-во на таблицу разновидностей количеств продукта(на главную сущность)
}
