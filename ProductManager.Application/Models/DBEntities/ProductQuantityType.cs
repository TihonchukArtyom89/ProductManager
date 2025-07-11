
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд для посвященной типам наименований количеств продукта (поштучно, по весу, по объёму)
[Table("ProductQuantityTypes")]
public class ProductQuantityType
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ProductQuantityTypeID { get; set; }//ид типа разновидности количества продукта//уникальный, первичный ключ
    [DisplayName("Количество продуктов в покупке")]
    [Required(ErrorMessage = "Введите название единицы количества продукта.")]
    [StringLength(20)]
    public string ProductQuantityTypeName { get; set; } = string.Empty;//название типа разновидности покупки   
    public List<ProductQuantity>? ProductQuantities { get; set; }//навигационное св-во на таблицу разновидностей количеств продукта(на зависимую сущность)
}
