using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой категориям продуктой
[Table("Categories")]
public class Category
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long? CategoryID { get; set; }//ИД категории продукта//уникальный,первичный ключ
    [DisplayName("Категория продукта")]
    [Required(ErrorMessage = "Category of product is required.")]
    public string CategoryName { get; set; } = string.Empty;//название категории продукта
    public List<Product>? Products { get; set; }//навигационное св-во на таблицу продуктов(на зависимую сущность)
}
