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
    [Required(ErrorMessage = "Выберите название типа опционального параметра.")]
    [StringLength(20, MinimumLength = 1)]
    public string OptionalParameterType { get; set; } = string.Empty; //Название название опционального параметра
    [DisplayName("Название опционального параметра")]
    [Required(ErrorMessage = "Введите название опционального параметра.")]
    [StringLength(50)]
    public string OptionalParameterName { get; set; } = string.Empty; //Название опционального параметра
    public List<PricelistOptionalParameter>? PricelistOptionalParameters { get; set; }//навигационное св-во на таблицу опциональных параметров конкретного прайслиста(на зависимую сущность)
}
