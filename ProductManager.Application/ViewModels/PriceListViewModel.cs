using ProductManager.Application.Models.DBEntities;
namespace ProductManager.Application.ViewModels;

public class PriceListViewModel : BaseListViewModel
{
    public PriceListViewModel()
    {
        PageViewModel = new() {};
        PageSizes = new int[] { 1, 2, 3, 5, 10 };
    }
    public IEnumerable<Pricelist> PriceLists { get; set; } = Enumerable.Empty<Pricelist>();
}
