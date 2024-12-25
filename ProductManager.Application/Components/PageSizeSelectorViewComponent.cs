using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManager.Application.Models;
using ProductManager.Application.ViewModels;

namespace ProductManager.Application.Components;

public class PageSizeSelectorViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(BaseListViewModel listViewModel)
    {
        ViewBag.SelectedPageSize = listViewModel.PageViewModel.PageSize;
        return View(listViewModel);
    }
}
