using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Drawing.Printing;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class ProductController : Controller
{
    private const int PAGES_RANGE_SIZE = 9;
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
    public async Task<IActionResult> Index(string? product, Guid? storeId, double? min, double? max, int page = 1, int pageSize = 9)
    {
        var stores = await storeService.GetAll();

        ViewBag.Stores = stores;
        ViewBag.storeId = storeId;
        ViewBag.product = product;
        ViewBag.min = min;
        ViewBag.max = max;

        ViewBag.MinPrice = 0;
        ViewBag.MaxPrice = double.MaxValue;

        Expression<Func<Product, bool>> filterExpression = p =>
            (product != null ? p.Name.ToLower().Contains(product.ToLower()) : true) &&
            (storeId != null ? p.StoreId == storeId : true) &&
            (min != null ? p.Price >= min : true) &&
            (max != null ? p.Price <= max : true);



        int totalPersons = await productService.Count(filterExpression);
        int totalPages = (int)Math.Ceiling((double)totalPersons / pageSize);

        int startPage = Math.Max(1, page - PAGES_RANGE_SIZE / 2);
        int endPage = Math.Min(totalPages, startPage + PAGES_RANGE_SIZE - 1);

        if (endPage - startPage + 1 < PAGES_RANGE_SIZE)
        {
            startPage = Math.Max(1, endPage - PAGES_RANGE_SIZE + 1);
        }        
        

        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = totalPages;
        ViewBag.StartPage = startPage;
        ViewBag.EndPage = endPage;
        ViewBag.ActionName = nameof(Index);

        ViewBag.RouteValues = new Dictionary<string, object>
        {
            ["min"] = ViewBag.min,
            ["max"] = ViewBag.max,
            ["storeId"] = ViewBag.storeId
        };

        var products = await productService.Filter(filterExpression, page, pageSize);

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
        ViewBag.CurrentPage = 1;
        ViewBag.PageSize = 10;
        ViewBag.TotalPages = 1;
        ViewBag.StartPage = 1;
        ViewBag.EndPage = 1;
        ViewBag.ActionName = nameof(Index);

        var top = await purchaseService.GetTopCustomersForProduct(id, 10);
        return View(top);
    }
}
