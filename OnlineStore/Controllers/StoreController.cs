using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services.Interfaces;

namespace OnlineStore.Controllers;
[Route("[controller]/[action]")]
public class StoreController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService productService;
    private readonly IStoreService storeService;

    public StoreController(ILogger<HomeController> logger, IProductService productService, IStoreService storeService)
    {
        _logger = logger;
        this.productService = productService;
        this.storeService = storeService;
    }

    public IActionResult Index()
    {
        return View();
    }
}
