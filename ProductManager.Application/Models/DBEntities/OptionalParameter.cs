using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой опциональным параметрам
[Table("OptionalParameters")]
public class OptionalParameter
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long OptionalParameterID { get; set; }//ИД опционального параметра//уникальный,первичный ключ
    [DisplayName("Тип опционального параметра")]
    [Required(ErrorMessage = "Type of optional parameter is required.")]
    public string OptionalParameterType { get; set; } = string.Empty; //Название типа опционального параметра
    [DisplayName("Название опционального параметра")]
    [Required(ErrorMessage = "Name of optional parameter is required.")]
    public string OptionalParameterName { get; set; } = string.Empty; //Название опционального параметра
    public PricelistOptionalParameter? PricelistOptionalParameter { get; set; }//навигационное св-во на таблицу опциональных параметров конкретного прайслиста(на главную сущность)
}
