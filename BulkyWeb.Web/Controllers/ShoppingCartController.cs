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

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int id)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == id);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == id);
            
            if (cartFromDb.Count == 1) 
            {
                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            } else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == id);
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
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
