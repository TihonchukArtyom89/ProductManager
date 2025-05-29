using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.ViewModels;

public class ProductsListViewModel : BaseListViewModel
{
    public ProductsListViewModel()
    {
        PageViewModel = new() {};
        PageSizes = new int[] { 1, 2, 3, 5, 10 };
    }
    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
    public string? CurrentCategory { get; set; }
}
