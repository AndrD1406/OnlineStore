using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Security.Claims;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class PurchaseController : Controller
{
    private readonly IPurchaseService _purchaseService;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public PurchaseController(IPurchaseService purchaseService, ICartService cartService, IProductService productService)
    {
        _purchaseService = purchaseService;
        _cartService = cartService;
        _productService = productService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized();

        var purchases = await _purchaseService.GetAll();
        var userPurchases = purchases.Where(p => p.UserId == userId);
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

        await _purchaseService.Create(purchase);

        await _cartService.ClearCart(cart.Id);

        return RedirectToAction(nameof(Index));
    }

}

