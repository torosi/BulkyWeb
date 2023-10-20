using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Domain.Models;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BulkyWeb.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category");
        // the product should be passed into view, it should be a list of view models
        return View(products);
    }    
    
    public IActionResult Details(int id)
    {
        ShoppingCart cart = new ShoppingCart()
        {
            Product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category"),
            Count = 1,
            ProductId = id
        };

        return View(cart);
    }

    [HttpPost]
    [Authorize] // we need this to get the user id of the logged in user
    public IActionResult Details(ShoppingCart cart)
    {
        // get the current user
        var claimsIdentity = (ClaimsIdentity)User.Identity; // convert to claims identity
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        cart.ApplicationUserId = userId;
        cart.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == cart.ProductId, includeProperties: "Category");

        _unitOfWork.ShoppingCartRepository.Add(cart);
        _unitOfWork.Save();

        TempData["success"] = "Cart updated successfully";

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

