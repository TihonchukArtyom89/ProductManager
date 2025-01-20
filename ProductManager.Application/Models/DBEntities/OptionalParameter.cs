using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования справочной таблицы для необязательных параметров (ид,тип, название)
public class OptionalParameter
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public long? OptionalParameterID { get; set; }//ид необязательного параметра для прайс листа//уникальный,первичный ключ
    [Required(ErrorMessage = "Type is required.")]
    public string OptionalParameterType { get; set; } = string.Empty;//тип необязательного параметра для прайс листа
    [Required(ErrorMessage = "Name of optional parameter is required.")]
    public string OptionalParameterName { get; set; } = string.Empty;//название  необязательного параметра для прайс листа
    public List<PriceListOptionalParameter> PriceListOptionalParameters { get; set; } = new();//навигационное свойство на таблицу необязательных параметров прайс листов
}
