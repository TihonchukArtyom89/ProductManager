using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной наименований количеств продукта(шт., кг., л.)
[Table("ProductQuantities")]
public class ProductQuantity
{

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? ProductQuantityID { get; set; }//ид разновидности количества продукта//уникальный, первичный ключ
    [DisplayName("Количество продуктов в покупке")]
    [Required(ErrorMessage = "Введите обозначение количества продуктов.")]
    [StringLength(5, MinimumLength = 3)]
    public string ProductQuantityName { get; set; } = string.Empty;//название разновидности покупки 
    public long? ProductQuantityTypeID { get; set; }//ид типа разновидности// внешний ключ на таблицу типов разновидностей количеств продуктов
    //[DeleteBehavior(DeleteBehavior.Cascade)]
    public ProductQuantityType? ProductQuantityType { get; set; }//навигационное св-во на таблицу типов разновидностей количеств продукта(на главную сущность)
    public List<Product>? Products { get; set; }//навигационное св-во на таблицу продуктов(на зависимую сущность)
}
