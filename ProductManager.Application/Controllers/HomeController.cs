using Microsoft.AspNetCore.Mvc;

namespace ProductManager.Application.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() =>View();
}
