using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.Models;

namespace ProductManager.Application.Controllers;

public class PricelistController : Controller
{
    private IPricelistRepository pricelistRepository;
    public PricelistController(IPricelistRepository _pricelistRepository)
    {
        pricelistRepository = _pricelistRepository;
    }
    public IActionResult Index()
    {
        return View();
    }
}
