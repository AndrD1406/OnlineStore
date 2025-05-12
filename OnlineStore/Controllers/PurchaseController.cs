using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private const int PAGES_RANGE_SIZE = 12;

    public PurchaseController(IPurchaseService purchaseService, ICartService cartService, IProductService productService)
    {
        _purchaseService = purchaseService;
        _cartService = cartService;
        _productService = productService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 12)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized();

        Expression<Func<Purchase, bool>> filterExpression = p => p.UserId == userId;

        int totalPersons = await _purchaseService.Count(filterExpression);
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

        var userPurchases = await _purchaseService.Filter(filterExpression, page, pageSize);
        return View(userPurchases);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized();

        var carts = await _cartService.GetByUserId(userId);
        var cart = carts.FirstOrDefault();
        if (cart == null || cart.ProductToCarts == null || !cart.ProductToCarts.Any())
        {
            TempData["Error"] = "Your cart is empty.";
            return RedirectToAction("Index", "Cart");
        }

        var groupedItems = cart.ProductToCarts
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                Product = g.First().Product,
                Quantity = g.Sum(x => x.Quantity)
            })
            .ToList();

        double totalSum = groupedItems.Sum(x => (x.Product?.Price ?? 0) * x.Quantity);
        List<string> errors = new List<string>();

        foreach (var item in groupedItems)
        {
            if (item.Product == null || item.Product.Quantity < item.Quantity)
                errors.Add($"Not enough stock for product: {item.Product?.Name}, you want to buy {item.Quantity}, but just {item?.Product?.Quantity} are available");
        }

        if (errors.Count > 0)
        {
            TempData["errors"] = errors;
            return RedirectToAction("Index", "Cart");
        }
        foreach (var item in groupedItems)
        {
            item.Product.Quantity -= item.Quantity;
            await _productService.UpdateProduct(item.Product.Id, item.Product);
        }

        var purchase = new Purchase
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedDate = DateTime.Now,
            TotalAmount = totalSum
        };

        await _purchaseService.CreateWithItems(purchase, groupedItems.Select(i => (i.Product.Id, i.Quantity, i.Product.Price)));

        await _cartService.ClearCart(cart.Id);

        return RedirectToAction(nameof(Index));
    }
}

