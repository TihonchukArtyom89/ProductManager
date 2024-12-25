using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Models;
using ProductManager.Application.ViewModels;
using System.Xml.Linq;

namespace ProductManager.Application.Components;

public class CategoryFilterViewComponent : ViewComponent
{
    private IProductRepository productRepository;
    public CategoryFilterViewComponent(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public IViewComponentResult Invoke()
    {
        return View(productRepository.Categories.Select(e => e.CategoryName).OrderBy(e => e));
    }
}
