using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;
using ProductManager.Application.ViewModels;
using System.Linq;

namespace ProductManager.Application.Controllers;

public class PricelistController : Controller
{
    private IPricelistRepository pricelistRepository;
    private int[] pricelistNumberOnPage = { 1, 2, 5, 10 };
    private int[] purchaseNumberOnPage = { 1, 2, 3, 5 };
    public PricelistController(IPricelistRepository _pricelistRepository)
    {
        pricelistRepository = _pricelistRepository;
    }
    public IActionResult PricelistList(string? searchString, SortOrder sortOrder = SortOrder.Neutral, int pricelistPage = 1, int pageSize = 0)
    {
        pageSize = pageSize == 0 ? pricelistNumberOnPage[0] : pageSize;
        ViewBag.SelectedPageSize = pageSize;
        ViewBag.DateCreationSortOrder = sortOrder == SortOrder.DateCreationDesc ? SortOrder.DateCreationAsc : SortOrder.DateCreationDesc;
        ViewBag.DateModificationSortOrder = sortOrder == SortOrder.DateModificationDesc ? SortOrder.DateModificationAsc : SortOrder.DateModificationDesc;
        ViewBag.NameSortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        ViewBag.DateCreationSortingText = sortOrder != SortOrder.DateCreationDesc ? "От поздних к ранним по дате создания" : "От ранних к поздним по дате создания";
        ViewBag.DateModificationSortingText = sortOrder != SortOrder.DateModificationDesc ? "От поздних к ранним по дате изменения" : "От ранних к поздним по дате изменения";
        ViewBag.NameSortingText = sortOrder != SortOrder.NameDesc ? "От Я до А" : "От А до Я";
        IEnumerable<Pricelist> pricelists = pricelistRepository.Pricelists;
        Pricelist SystemPricelist = SystemValues.GetPricelistNull();
        int totalItems = pricelists.Count();
        if (!String.IsNullOrEmpty(searchString))
        {
            ViewBag.SearchString = searchString;
            SystemPricelist = SystemValues.GetPricelistNotFound(ViewBag.SearchString);
            pricelists = pricelists.Where(e => e.PricelistName.ToLower().Contains(searchString.ToLower()));
            totalItems = pricelists.Count();
        }
        switch (sortOrder)
        {
            case SortOrder.DateModificationAsc:
                pricelists = pricelists.OrderBy(e => e.PriceListDateModification);
                ViewBag.SortOrder = ViewBag.DateModificationSortOrder;
                break;
            case SortOrder.DateModificationDesc:
                pricelists = pricelists.OrderByDescending(e => e.PriceListDateModification);
                ViewBag.SortOrder = ViewBag.DateModificationSortOrder;
                break;
            case SortOrder.DateCreationAsc:
                pricelists = pricelists.OrderBy(e => e.PriceListDateCreation);
                ViewBag.SortOrder = ViewBag.DateCreationSortOrder;
                break;
            case SortOrder.DateCreationDesc:
                pricelists = pricelists.OrderByDescending(e => e.PriceListDateCreation);
                ViewBag.SortOrder = ViewBag.DateCreationSortOrder;
                break;
            case SortOrder.NameAsc:
                pricelists = pricelists.OrderBy(e => e.PricelistName);
                ViewBag.SortOrder = ViewBag.NameSortOrder;
                break;
            case SortOrder.NameDesc:
                pricelists = pricelists.OrderByDescending(e => e.PricelistName);
                ViewBag.SortOrder = ViewBag.NameSortOrder;
                break;
            default:
                pricelists = pricelists.OrderBy(e => e.PricelistID);
                ViewBag.SortOrder = SortOrder.Neutral;
                break;
        }
        if (pricelists.Count() == 0 && pricelistPage != 1)
        {
            pricelistPage = 1; //если нет прайслистов, то сбрасываем страницу на 1
            pricelists = pricelistRepository.Pricelists.OrderBy(p => p.PricelistID);
        }
        if (pricelistPage > (int)Math.Ceiling((decimal)totalItems / pageSize))
        {
            pricelistPage = 1;
        }
        pricelists = pricelists.Skip((pricelistPage - 1) * pageSize).Take(pageSize);
        ViewBag.PricelistCount = pricelists.Count();
        ViewBag.SelectedPage = pricelistPage;
        pricelists = pricelists.Count() != 0 ? pricelists : pricelists.Append(SystemPricelist);
        PricelistListViewModel viewModel = new PricelistListViewModel
        {
            Pricelists = pricelists,
            PageViewModel = new PageViewModel
            {
                CurrentPage = ViewBag.SelectedPage,
                PageSize = pageSize,
                TotalItems = totalItems                
            },            
            ControllerName = ControllerContext.ActionDescriptor.ControllerName ?? "",
            ActionName = ControllerContext.ActionDescriptor.ActionName ?? "",
            PageSizes = pricelistNumberOnPage,
            SizeSelectorText = "Выберите количество отображаемых прайслистов: "
        };
        return View(viewModel);
    }
    public SortOrder SaveSortOrderState(SortOrder sortOrder)
    {

        if (sortOrder == SortOrder.NameAsc || sortOrder == SortOrder.NameDesc)
        {
            sortOrder = sortOrder == SortOrder.NameDesc ? SortOrder.NameAsc : SortOrder.NameDesc;
        }
        if (sortOrder == SortOrder.DateCreationAsc || sortOrder == SortOrder.DateCreationDesc)
        {
            sortOrder = sortOrder == SortOrder.DateCreationDesc ? SortOrder.DateCreationAsc : SortOrder.DateCreationDesc;
        }
        if (sortOrder == SortOrder.DateModificationAsc || sortOrder == SortOrder.DateModificationDesc)
        {
            sortOrder = sortOrder == SortOrder.DateModificationDesc ? SortOrder.DateModificationAsc : SortOrder.DateModificationDesc;
        }
        return sortOrder;
    }
    public IActionResult PricelistPage(long? pricelistId = 0, int purchasePage = 1, int pageSize = 0)
    {
        //действие для отображения страницы прайслиста с продуктами и дополнительными параметрами в нём
        //в передаваемом параметре будет передаваться ИД прайслиста, который нужно отобразить(наверное на данном этапе это всё)
        //сделать модель представления для страницы прайслиста, которая будет содержать в себе список продуктов и список опциональных параметров
        ViewBag.SelectedPricelistId = pricelistId;
        Pricelist pricelist = pricelistRepository.Pricelists.Where(e => e.PricelistID == pricelistId).FirstOrDefault() ?? SystemValues.GetPricelistNull();//получение прайлиста по ид
        List<PricelistProductPurchase> purchases = new List<PricelistProductPurchase>();
        List<PricelistProductPurchase> bufferPurchases = new List<PricelistProductPurchase>();
        List<PricelistOptionalParameter> optionalParameterValues = new List<PricelistOptionalParameter>();
        foreach (PricelistProductPurchase purchase in pricelistRepository.PricelistProductPurchases)
        {//проход по всем покупками,что бы выбрать те которые принадлежат данному прайслисту
            if (pricelist.PricelistID == purchase.PricelistID)
            {
                purchases.Add(purchase);
                foreach (PricelistOptionalParameter value in pricelistRepository.PricelistOptionalParameters)
                {//проход по всем значениям опциональных параметров,что бы выбрать те которые принадлежат данной покупке в данном прайслисте
                    if (purchase.PurchaseID == value.PurchaseID)
                    {
                        optionalParameterValues.Add(value);
                    }
                }
            }
        }
        bufferPurchases = purchases;
        pageSize = (pageSize == 0 || purchases.Count() == 0) ? purchaseNumberOnPage[0] : pageSize;
        ViewBag.SelectedPurchasePageSize = pageSize;
        List<long?> optionalParameterUniqueID = optionalParameterValues.Select(e => e.OptionalParameterID).Distinct().ToList();//выбор уникальных ИД прайслиста
        List<string> optionalParameterNames = new List<string>();
        foreach (OptionalParameter parameter in pricelistRepository.OptionalParameters)
        {
            if (optionalParameterUniqueID.Contains(parameter.OptionalParameterID))
            {
                optionalParameterNames.Add(parameter.OptionalParameterName);
            }
        }
        List<Product> products = new List<Product>();
        foreach (Product product in pricelistRepository.Products)
        {//проход по всем продуктам (тип данных продуктов),что бы выбрать те которые принадлежат данному прайслисту
            if (purchases.Contains(purchases.Where(e => e.ProductID == product.ProductID).FirstOrDefault() ?? new PricelistProductPurchase() { ProductID = null }))
            {
                products.Add(product);
            }
        }
        int totalItems = purchases.Count();
        ViewBag.PricelistCategories = new SelectList(pricelistRepository.Categories, "CategoryID", "CategoryName");
        if (purchases.Count() == 0 && purchasePage != 1)
        {
            purchasePage = 1;
            purchases = bufferPurchases;
        }
        if (purchasePage > (int)Math.Ceiling((decimal)totalItems / pageSize))
        {//check if current page is more than pages 
            purchasePage = 1;
        }
        purchases = purchases.Skip((purchasePage - 1) * pageSize).Take(pageSize).ToList();
        ViewBag.SelectedPageSize = pageSize;
        ViewBag.SelectedPurchasePage = purchasePage;
        PricelistPageViewModel viewModel = new PricelistPageViewModel
        {
            PageViewModel = new PageViewModel 
            {
                CurrentPage = purchasePage,
                PageSize = pageSize,
                TotalItems = totalItems
            },
            Pricelist = pricelist,
            Purchases = purchases,
            PurchaseListViewModel = new PurchaseListViewModel()
            {
                Purchases = purchases,
                PageViewModel = new PageViewModel 
                {
                    CurrentPage = purchasePage,
                    PageSize = pageSize,
                    TotalItems = totalItems
                },
                ControllerName = ControllerContext.ActionDescriptor.ControllerName ?? "",
                ActionName = ControllerContext.ActionDescriptor.ActionName ?? "",
                PageSizes = purchaseNumberOnPage,
                SizeSelectorText = "Выберите количество отображаемых покупок: "
            },
            PricelistProducts = products,
            OptionalParameterNames = optionalParameterNames,
            OptionalParameterValues = optionalParameterValues
        };
        return View(viewModel);
    }
}
