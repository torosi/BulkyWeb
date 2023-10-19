using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Domain.Models;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.ViewModels;

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
        Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category");
        ProductVM productVM = new ProductVM()
        {
            Title = product.Title,
            ISBN = product.ISBN,
            Author = product.Author,
            Description = product.Description,
            ListPrice = product.ListPrice,
            Price = product.Price,
            Price50 = product.Price50,
            Price100 = product.Price100,
            CategoryId = product.CategoryId,
            ImageUrl = product.ImageUrl,
            Id = product.Id
        };

        return View(productVM);
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

