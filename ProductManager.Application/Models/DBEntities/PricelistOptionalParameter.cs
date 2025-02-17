using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой опциональным параметрам конкретного прайслиста 
[Table("PricelistOptionalParameters")]
public class PricelistOptionalParameter
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OptionalParameterEntryID { get; set; } //ИД опционального параметра для конкретного прайслиста//уникальный,первичный ключ
    public long OptionalParameterID { get; set; }
    [DisplayName("Значение опционального параметра")]
    [Required(ErrorMessage = "Value of optional parameter is required.")]
    public string OptionalParameterValue { get; set; } = string.Empty;
    public long PricelistID { get; set; }//ИД прайслиста для данного опционального параметра  
    public Pricelist Pricelist { get; set; } = new Pricelist();//навигационное св-во на таблицу прайслистов(на главную сущность)
    public List<OptionalParameter> OptionalParameters { get; set; } = new();//навигационное св-во на таблицу опциональных параметров(на зависимую сущность)
}
