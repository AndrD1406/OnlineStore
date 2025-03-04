using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.DataAccess.Models;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public ProductController(ILogger<HomeController> logger, IProductService prdService)
        {
            _logger = logger;
            productService = prdService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetProductsByStore(Guid storeId)
        {
            var products = await productService.GetProductsByStore(storeId);
            return View(products);
        }
    }
}
