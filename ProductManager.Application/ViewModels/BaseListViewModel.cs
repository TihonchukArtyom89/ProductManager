using ProductManager.Application.Models;


namespace ProductManager.Application.ViewModels;

public class BaseListViewModel
{
    public PageViewModel PageViewModel { get; set; }
    public int[] PageSizes { get; set; } 
    public string? ControllerName { get; set; } = string.Empty;
    public string? ActionName { get; set; } = string.Empty;
    public string? SizeSelectorText { get; set;} = string.Empty;
    //public string? text { get; set; }
    public BaseListViewModel()
    {
        PageViewModel = new PageViewModel();
        PageSizes = new int[] { 1, 2, 3, 5, 10 };
        SizeSelectorText = "Выберите количество ";
        //text = "class";
    }
}
