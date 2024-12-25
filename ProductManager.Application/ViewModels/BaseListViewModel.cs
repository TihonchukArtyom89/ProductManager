using ProductManager.Application.Models;


namespace ProductManager.Application.ViewModels;

public class BaseListViewModel
{
    public PageViewModel PageViewModel { get; set; } = new();
    public int[] PageSizes = new int[] { };
}
