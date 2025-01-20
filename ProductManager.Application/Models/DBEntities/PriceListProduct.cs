using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы с обязательными значениями для прайс листов (ид позиции прайс листа, ид прайс листа, ид товара) 
public class PriceListProduct
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long? PriceListLineID { get; set; }//ид определённой позиции прайс листа для указанного товара//уникальный,первичный ключ
    //[ForeignKey("PriceListLineID")]
    public PriceListOptionalParameter PriceListOptionalParameters { get; set; } = new();//навигационное св-во на таблицу необязательных параметров прайс листа
    [Required]
    public long? PriceListID { get; set; }//ид прайс листа//внешний ключ
    [Required]
    public List<PriceList> PriceLists { get; set; } = new();//навигационное св-во на таблицу прайс листов    
    [Required]
    public long? ProductID { get; set; }//ид товара в прайс листе//внешний ключ
    [Required]
    public List<Product> Products { get; set; } = new();//навигационное св-во на таблицу продуктов
}
