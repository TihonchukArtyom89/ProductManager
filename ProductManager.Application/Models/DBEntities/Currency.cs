using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой денежной валюте для цен на продукты
[Table("Currencies")]
public class Currency
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? CurrencyID { get; set; }//ИД денежной валюты//уникальный,первичный ключ
    [DisplayName("Полное название денежной валюты")]
    [Required(ErrorMessage = "Full name of currency is required.")]
    [StringLength(50,MinimumLength = 5)]
    public string CurrencyFullName { get; set; } = string.Empty;//полное название денежной валюты(для crud в панели администратора)Пример: Российский рубль
    [DisplayName("Сокращённое название денежной валюты")]
    [Required(ErrorMessage = "Short name of currency is required.")]
    [StringLength(5, MinimumLength = 3)]
    public string CurrencyShortName { get; set; } = string.Empty;//сокращённое название денежной валюты(для прайс листов)Пример: руб.
    [DisplayName("Символ денежной валюты")]
    [Required(ErrorMessage = "Symbol of currency is required.")]
    [StringLength(1)]
    public string CurrencySymbol {  get; set; } = string.Empty;//символ денежной валюты(для списка продуктов)Пример: ₽
    public List<Product>? Products { get; set; }//навигационное св-во на таблицу продуктов(на зависимую сущность)
    public Currency()
    {
        CurrencyFullName = "Российский рубль";
        CurrencyShortName = "руб.";
        CurrencySymbol = "₽";
    }
}
