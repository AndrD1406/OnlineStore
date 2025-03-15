using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Runtime.CompilerServices;

namespace OnlineStore.Controllers
{
    [Route("onlinestore/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;

        public ProductController(ILogger<HomeController> logger, IProductService prdService)
        {
            _logger = logger;
            productService = prdService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetProducts();

            return View(products);
        }
        [HttpGet]
        [Route("{storeId}")]
        public async Task<IActionResult> GetProductsByStore(Guid storeId)
        {
            var products = await productService.GetProductsByStore(storeId);
            return View(nameof(GetProductsByStore), products);
        }
    }
}
