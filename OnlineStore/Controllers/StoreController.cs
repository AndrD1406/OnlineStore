using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class StoreController : Controller
{
    private readonly IProductService productService;
    private readonly IStoreService storeService;

    public StoreController(IProductService productService, IStoreService storeService)
    {
        this.productService = productService;
        this.storeService = storeService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var stores = await storeService.GetAll();
        return View(stores);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid storeId, string? product, double? price)
    {
        var store = await storeService.Get(storeId);
        if (store == null)
        {
            return NotFound();
        }

        ViewBag.storeId = storeId;
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
    [Authorize(Roles ="User", AuthenticationSchemes =CookieAuthenticationDefaults.AuthenticationScheme)]
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
   public async Task<IActionResult> DeleteProduct(Guid productId, Guid storeId)
    {
        if (productId == Guid.Empty || storeId == Guid.Empty)
        {
            ModelState.AddModelError("", "Incorrect data.");
            return RedirectToAction("Details", new { storeId = storeId });
        }

        if (!User.IsInRole("Admin"))
        {
            ModelState.AddModelError("", "You do not have permission to delete the product.");
            return RedirectToAction("Details", new { storeId = storeId });
        }

        try
        {
            await productService.Delete(productId);
            return RedirectToAction("Details", new { storeId = storeId });
        }
        catch
        {
            ModelState.AddModelError("", "Error when deleting a product.");
            return RedirectToAction("Details", new { storeId = storeId });
        }
    }

}
