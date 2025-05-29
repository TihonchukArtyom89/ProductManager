using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой прайслистам покупок продуктов
[Table("Pricelists")]
public class Pricelist
{
    public long PricelistId { get; set; } //ИД прайслиста покупок продуктов//уникальный, первичный ключ
    [Required(ErrorMessage = "Введите название прайслиста.")]
    [DisplayName("Название прайслиста")]
    [StringLength(50)]
    public string PricelistName { get; set; } = string.Empty; //Название прайслиста покупок продуктов
    public DateTime PriceListDateCreation { get; set; }//Дата и время создания прайслиста покупок продуктов
    public DateTime PriceListDateModification { get; set; }//Дата и время изменения прайслиста покупок продуктов
    public List<PricelistProductPurchase>? PricelistProductPurchases { get; set; }//навигационное св-во на таблицу покупок продуктов(на зависимую сущность)
    public List<PricelistOptionalParameter>? PricelistOptionalParameters { get; set; }//навигационное св-во на таблицу опциональных параметров прайслиста(на зависимую сущность)
}
