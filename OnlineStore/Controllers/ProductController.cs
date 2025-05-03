using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Runtime.CompilerServices;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class ProductController : Controller
{
    private readonly IProductService productService;
    private readonly IStoreService storeService;
    private readonly IPurchaseService purchaseService;

    public ProductController(
        IProductService prdService,
        IStoreService strService,
        IPurchaseService purchaseService)
    {
        this.productService = prdService;
        this.storeService = strService;
        this.purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? product, Guid? storeId, double? min, double? max, int page = 0, int pageSize = 9)
    {
        var stores = await storeService.GetAll();

        ViewBag.Stores = stores;
        ViewBag.storeId = storeId;
        ViewBag.product = product;
        ViewBag.min = min;
        ViewBag.max = max;

        ViewBag.MinPrice = 0;
        ViewBag.MaxPrice = double.MaxValue;

        var products = await productService.Filter(p =>
            (product != null ? p.Name.ToLower().Contains(product.ToLower()) : true) &&
            (storeId != null ? p.StoreId == storeId : true) &&
            (min != null ? p.Price >= min : true) &&
            (max != null ? p.Price <= max : true), page, pageSize);

        return View(products);
    }

    [HttpGet]
    [Route("{storeId}")]
    public async Task<IActionResult> GetProductsByStore(Guid storeId)
    {
        var products = await productService.GetByStore(storeId);
        return View(nameof(GetProductsByStore), products);
    }

    [HttpGet]
    public async Task<IActionResult> Details([FromQuery] Guid id)
    {
        var product = await productService.Filter(x => x.Id == id);
        return View(product.FirstOrDefault());
    }

    [HttpGet]
    public async Task<IActionResult> TopBuyers(Guid id)
    {
        var top = await purchaseService.GetTopCustomersForProduct(id, 10);
        return View(top);
    }
}
