using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.ViewModels;

public class PricelistPageViewModel : BaseListViewModel
{
    public Pricelist? Pricelist { get; set; }
    public List<PricelistProductPurchase>? Purchases { get; set; }
}
