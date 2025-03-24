using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add()
    {
        return View();
    }
}
