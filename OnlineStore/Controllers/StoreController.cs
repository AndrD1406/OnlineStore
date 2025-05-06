using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.Models;
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class StoreController : Controller
{
    private const int PAGES_RANGE_SIZE = 9;
    private readonly IProductService productService;
    private readonly IStoreService storeService;

    public StoreController(IProductService productService, IStoreService storeService)
    {
        this.productService = productService;
        this.storeService = storeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
    {
        int totalStores = await storeService.Count(x => true);
        int totalPages = (int)Math.Ceiling((double)totalStores / pageSize);

        int startPage = Math.Max(1, page - 2);
        int endPage = Math.Min(totalPages, page + 2);

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


        var stores = await storeService.Filter(x => true, page, pageSize);
        return View(stores);
    }

    public async Task<IActionResult> Details(Guid storeId, string? product, double? price, int page = 1)
    {
        const int pageSize = 6;  

        var store = await storeService.Get(storeId);
        if (store == null) return NotFound();

        ViewBag.storeId = storeId;
        ViewBag.ProductFilter = product;
        ViewBag.PriceFilter = price;

        Expression<Func<Product, bool>> filter = x =>
            x.StoreId == storeId &&
            (string.IsNullOrEmpty(product) || x.Name.ToLower().Contains(product.ToLower())) &&
            (!price.HasValue || x.Price <= price);

        var totalItems = await productService.Count(filter);
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        int startPage = Math.Max(1, page - 2);
        int endPage = Math.Min(totalPages, startPage + 4);
        if (endPage - startPage < 4)
            startPage = Math.Max(1, endPage - 4);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.StartPage = startPage;
        ViewBag.EndPage = endPage;
        ViewBag.ActionName = nameof(Details);
        ViewBag.RouteValues = new { storeId, product, price };

        var products = await productService.Filter(filter, page, pageSize);

        var vm = new StoreDetailsViewModel
        {
            Store = store,
            Products = products,
            ProductFilter = product,
            PriceFilter = price
        };
        return View(vm);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Store());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Store store)
    {
        if (!ModelState.IsValid)
        {
            return View(store);
        }

        await storeService.Create(store);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(Guid id)
    {
        var store = await storeService.Get(id);
        if (store == null)
        {
            return NotFound();
        }
        return View(store);
    }

    [HttpPost]
    [Authorize(Roles = "User", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Edit(Guid id, Store store)
    {
        if (!ModelState.IsValid)
        {
            return View(store);
        }

        await storeService.Update(id, store);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var store = await storeService.Get(id);
        if (store == null)
        {
            return NotFound();
        }
        return View(store);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await storeService.Delete(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult CreateProduct(Guid storeId)
    {
        ViewBag.StoreId = storeId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product model)
    {
        if (ModelState.IsValid)
        {
            model.Id = Guid.NewGuid();
            await productService.Create(model);
            return RedirectToAction("Details", "Store", new { storeId = model.StoreId });
        }

        return View(model);
    }

    // GET: /Store/DeleteProduct?storeId={storeId}&productId={productId}
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> DeleteProduct(Guid storeId, Guid productId)
    {
        // Перевіряємо, що магазин існує
        var store = await storeService.Get(storeId);
        if (store == null)
        {
            return NotFound($"Store with id {storeId} not found");
        }

        // Шукаємо товар у цьому магазині
        var products = await productService.Filter(p => p.Id == productId && p.StoreId == storeId);
        var product = products.FirstOrDefault();
        if (product == null)
        {
            return NotFound($"Product with id {productId} not found in this store");
        }

        // Видаляємо товар
        await productService.Delete(productId);

        // Повертаємося на Details магазину
        return RedirectToAction(nameof(Details), new { storeId = storeId });
    }

}
