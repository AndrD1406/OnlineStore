using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.BusinessLogic.Services;
using OnlineStore.BusinessLogic.Services.Interfaces;
using OnlineStore.DataAccess.Models;
using System.Security.Claims;

namespace OnlineStore.Controllers;

[Route("[controller]/[action]")]
public class CartController : Controller
{

    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        this._cartService = cartService;
    }
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized();
        }

        var carts = await _cartService.GetByUserId(userId);
        var cart = carts.FirstOrDefault();
        if (cart == null)
        {
            cart = await _cartService.Create(new Cart { Id = Guid.NewGuid(), UserId = userId });
        }

        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Add(Guid productId, int quantity)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
        {
            return Unauthorized();
        }

        var userCarts = await _cartService.GetByUserId(userId);
        var cart = userCarts.FirstOrDefault();

        if (cart == null)
        {
            cart = await _cartService.Create(new Cart { Id = Guid.NewGuid(), UserId = userId });
        }

        var cartProduct = new ProductToCart
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            CartId = cart.Id,
            Quantity = quantity
        };

        await _cartService.AddProductToCart(cart.Id, cartProduct);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Remove(Guid productId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out Guid userId))
            return Unauthorized();

        var carts = await _cartService.GetByUserId(userId);
        var cart = carts.FirstOrDefault();
        if (cart == null)
        {
            TempData["Error"] = "Cart not found.";
            return RedirectToAction("Index");
        }
        await _cartService.RemoveProductFromCart(cart.Id, productId);

        return RedirectToAction("Index");
    }
}
