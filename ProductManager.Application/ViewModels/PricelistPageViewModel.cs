﻿using ProductManager.Application.Models.DBEntities;

namespace ProductManager.Application.ViewModels;

public class PricelistPageViewModel : BaseListViewModel
{
    public PricelistPageViewModel()
    {
        PageViewModel = new BaseListViewModel().PageViewModel;
        PageSizes = new BaseListViewModel().PageSizes;
    }
    public Pricelist? Pricelist { get; set; }
    public List<PricelistProductPurchase>? Purchases { get; set; }
    public List<Product>? PricelistProducts { get; set; }
    public List<String>? OptionalParameterNames { get; set; }
    public List<PricelistOptionalParameter>? OptionalParameterValues { get; set; }
    public PurchaseListViewModel? PurchaseListViewModel { get; set; }
}
