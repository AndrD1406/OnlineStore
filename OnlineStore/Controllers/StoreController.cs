using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.Models;

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
    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> Index()
    {
        var stores = await storeService.GetAll();
        return View(stores);
    }

    public async Task<IActionResult> Details(Guid storeId, string? product, double? price)
    {
        var store = await storeService.Get(storeId);
        if (store == null)
        {
            return NotFound();
        }

        var products = await productService.Filter(x =>
            x.StoreId == storeId &&
            (string.IsNullOrEmpty(product) || x.Name.ToLower().Contains(product.ToLower())) &&
            (!price.HasValue || x.Price <= price));

        var viewModel = new StoreDetailsViewModel
        {
            Store = store,
            Products = products,
            ProductFilter = product,
            PriceFilter = price
        };

        return View(viewModel);
    }
}
