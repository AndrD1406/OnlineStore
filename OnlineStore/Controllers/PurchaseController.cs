using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class PurchaseController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
