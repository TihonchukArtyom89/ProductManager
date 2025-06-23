using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой конкретного опционального параметра конкретной покупки прайслиста 
[Table("PricelistOptionalParameters")]
public class PricelistOptionalParameter
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OptionalParameterEntryID { get; set; }//ИД значения конкретного опционального параметра для конкретного прайслиста//уникальный,первичный ключ
    public long OptionalParameterID { get; set; }
    [DisplayName("Значение опционального параметра")]
    [Required(ErrorMessage = "Введите значение опцинального параметра")]
    [StringLength(50)]
    public string OptionalParameterValue { get; set; } = string.Empty;
    public long PricelistPurchaseID { get; set; }//ИД покупки в прайслисте для данного опционального параметра  
    public Pricelist? Pricelist { get; set; }//навигационное св-во на таблицу прайслистов(на главную сущность)
    public OptionalParameter? OptionalParameter { get; set; }//навигационное св-во на таблицу опциональных параметров(на главную сущность)
    public PricelistProductPurchase? PricelistProductPurchase { get; set; }//навигационное св-во на таблицу покупок продуктов(на зависимую сущность)
}
