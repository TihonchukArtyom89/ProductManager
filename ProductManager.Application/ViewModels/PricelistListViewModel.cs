using ProductManager.Application.Models.DBEntities;
namespace ProductManager.Application.ViewModels;

public class PricelistListViewModel : BaseListViewModel
{
    public PricelistListViewModel()
    {
        PageViewModel = new BaseListViewModel().PageViewModel;
        PageSizes = new BaseListViewModel().PageSizes;
    }
    public IEnumerable<Pricelist> Pricelists { get; set; } = Enumerable.Empty<Pricelist>();
}
