using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManager.Application.Models.DBEntities;
//класс для формирования таблицы в бд посвящённой удалённым записям из любых других таблиц бд 
[Table("DeletedRecords")]
public class DeletedRecord
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long? DeletedRecordID { get; set; }//ИД удалённой записи
    [Required]
    public DateTime? RecordDeleteDateTime { get; set; }//Время и дата удаления записи
    [Required]
    public string TableSourceName { get; set; } = String.Empty;//Название таблицы, где находилась удаляемая запись

    [Required]
    public long? TableSourceDeletedRecordID { get; set; }//ИД удалённой записи в таблице, где находилась удаляемая запись 

    [Required]
    public string TableSourceDeletedRecordValueFromJSON { get; set; } = String.Empty;//Значение удалённой записи в таблице, где находилась удаляемая запись в формате JSON, приведённая к строковому типу
}
