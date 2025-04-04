﻿using Microsoft.AspNetCore.Authentication.Cookies;
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

    public ProductController(IProductService prdService, IStoreService strService)
    {
        productService = prdService;
        storeService = strService;
    }
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] string? product, [FromQuery] Guid? storeId, [FromQuery] double? price)
    {
        var stores = await storeService.GetAll();
        ViewBag.Stores = stores;
        ViewBag.storeId = storeId;
        ViewBag.product = product;
        ViewBag.price = price;
        var products = await productService.Filter(x => (product != null ? x.Name.ToLower().Contains(product.ToLower()) : true) && (storeId != null ? x.StoreId == storeId : true) && (price != null ? x.Price <= price : true));
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
}
