using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой категориям товарой
[Table("Categories")]
public class Category
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long? CategoryID { get; set; }//ИД категории товара//уникальный,первичный ключ
    [DisplayName("Категория продукта")]
    [Required(ErrorMessage = "Category of product is required.")]
    public string CategoryName { get; set; } = string.Empty;//название категории товара
    public List<Product> Products { get; set; } = new();//навигационное св-во на таблицу продуктов(на зависимую сущность)
}
