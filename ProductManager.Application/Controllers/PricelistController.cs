using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;
using ProductManager.Application.ViewModels;

namespace ProductManager.Application.Controllers;

public class PricelistController : Controller
{
    private IPricelistRepository pricelistRepository;
    public PricelistController(IPricelistRepository _pricelistRepository)
    {
        pricelistRepository = _pricelistRepository;
    }
    public IActionResult PricelistList(string? searchString, SortOrder sortOrder = SortOrder.Neutral, int pricelistPage = 1, int pageSize = 1)
    {
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
                pricelists = pricelists.OrderBy(e => e.PricelistId);
                ViewBag.SortOrder = SortOrder.Neutral;
                break;
        }
        if (pricelists.Count() == 0 && pricelistPage != 1)
        {
            pricelistPage = 1; //если нет прайслистов, то сбрасываем страницу на 1
            pricelists = pricelistRepository.Pricelists.OrderBy(p => p.PricelistId);
        }
        if (pricelistPage > (int)Math.Ceiling((decimal)totalItems / pageSize))
        {
            pricelistPage = 1;
        }
        pricelists = pricelists.Skip((pricelistPage - 1) * pageSize).Take(pageSize);
        ViewBag.PricelistCount = pricelists.Count();
        ViewBag.SelectedPage = pricelistPage;
        pricelists = pricelists.Count() != 0 ? pricelists : pricelists.Append(SystemPricelist);
        PriceListViewModel viewModel = new PriceListViewModel
        {
            PriceLists = pricelists,
            PageViewModel = new PageViewModel
            {
                CurrentPage = ViewBag.SelectedPage,
                PageSize = pageSize,
                TotalItems = totalItems
            },
            ControllerName = ControllerContext.ActionDescriptor.ControllerName ?? "",
            ActionName = ControllerContext.ActionDescriptor.ActionName ?? "",
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
    public IActionResult PricelistPage()
    {
        //действие для отображения страницы прайслиста с продуктами и дополнительными параметрами в нём
        //в передаваемом параметре будет передаваться ИД прайслиста, который нужно отобразить(наверное на данном этапе это всё)
        //сделать модель представления для страницы прайслиста, которая будет содержать в себе список продуктов и список опциональных параметров
        //изменить количество отображаемых продуктов на странице прайслиста с 1,2,5,10 на 5,10,20,50,100
        return View();
    }
}
