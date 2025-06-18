using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.ViewModels;

public class ProductsListViewModel : BaseListViewModel
{
    public ProductsListViewModel()
    {
        PageViewModel = new BaseListViewModel().PageViewModel;
        PageSizes = new BaseListViewModel().PageSizes;
    }
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public string? CurrentCategory { get; set; }
}
