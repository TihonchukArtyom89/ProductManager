
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной типам разновидности количеств продукта(поштучно, по весу, по объёму)
[Table("ProductQuantityTypes")]
public class ProductQuantityType
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ProductQuantityTypeID { get; set; }//ид типа разновидности количества продукта//уникальный, первичный ключ    [DisplayName("Количество продуктов в покупке")]
    [Required(ErrorMessage = "Quantity name of product is required")]
    public string ProductQuantityTypeName { get; set; } = string.Empty;//название типа разновидности покупки   
    public List<ProductQuantity>? ProductQuantities { get; set; }//навигационное св-во на таблицу разновидностей количеств продукта(на зависимую сущность)
}
