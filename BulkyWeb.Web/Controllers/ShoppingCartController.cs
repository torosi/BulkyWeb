using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;
using BulkyWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties:"Product"),
            };

            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                double price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (price * cart.Count);
            }


            return View(ShoppingCartVM);
        }


        private double GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            if (cart.Count <= 50)
            {
                return cart.Product.Price;
            } else if (cart.Count <= 100)
            {
                return cart.Product.Price50;
            } else
            {
                return cart.Product.Price100;
            }
        }


    }
}
