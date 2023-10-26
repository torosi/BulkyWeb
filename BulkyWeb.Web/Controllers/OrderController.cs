using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;
using BulkyWeb.Domain.ViewModels;
using BulkyWeb.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyWeb.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetailRepository.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };
            return View();
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();

            switch (status)
            {
                case "pending":
                    orders = orders.Where(u => u.OrderStatus == StaticDetails.StatusPending);
                    break;
                case "inprocess":
                    orders = orders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "approved":
                    orders = orders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "completed":
                    orders = orders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orders });
        }

        #endregion

    }
}
