using ProductManager.Application.Models.DBEntities;
namespace ProductManager.Application.ViewModels;

public class PriceListViewModel : BaseListViewModel
{
    public PriceListViewModel()
    {
        PageViewModel = new BaseListViewModel().PageViewModel;
        PageSizes = new BaseListViewModel().PageSizes;
    }
    public IEnumerable<Pricelist> PriceLists { get; set; } = Enumerable.Empty<Pricelist>();
}
