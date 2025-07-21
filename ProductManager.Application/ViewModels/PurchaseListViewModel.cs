using ProductManager.Application.Models.DBEntities;
namespace ProductManager.Application.ViewModels;

public class PurchaseListViewModel : BaseListViewModel
{
    public PurchaseListViewModel()
    {
        PageViewModel = new BaseListViewModel().PageViewModel;
        PageSizes = new BaseListViewModel().PageSizes;  
    }
    public IEnumerable<PricelistProductPurchase> Purchases { get; set; } = Enumerable.Empty<PricelistProductPurchase>();
}
