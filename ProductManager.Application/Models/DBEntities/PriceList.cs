using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой прайс-листам товаров
public class PriceList
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long? PriceListID { get; set; } //ИД прайс листа//уникальный,первичный ключ
    [Required(ErrorMessage = "Name of pricelist is required.")]
    public string PriceListName { get; set; } = string.Empty; //название прайс листа
    public DateTime PriceListDateCreation { get; set; }  //дата создания прайс листа
    public DateTime PriceListDateModyfycation { get; set; } //дата изменения прайс листа
    //[ForeignKey("PriceListID")]
    public List<PriceListProduct> PriceListProducts { get; set; } = new();
}
