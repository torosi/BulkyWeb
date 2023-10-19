using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;
using BulkyWeb.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulkyWeb.Web.Controllers
{
    public class ProductController : Controller
    {

        // 1) inject unit of work
        // 2) index action method
        // 3) create, update, delete action methods + create the views
        // 4) add client side and server side validations

        // 1) inject unit of work
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // 2) index action method
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties:"Category").ToList();
            // rather than passing a whole category to view we can pick out the parts we want and create a new object that can be passed
 
            return View(products);
        }

        // 3.1) create action method
        // GET
        public IActionResult Upsert(int? id)
        {

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.CategoryRepository
                 .GetAll().Select(u => new SelectListItem
                 {
                     Text = u.Name,
                     Value = u.Id.ToString()
                 });

            ProductVM productVM = new ProductVM()
            {
                CategoryList = CategoryList
            };

            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            { 
                Product product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);
                productVM.Title = product.Title;
                productVM.Description = product.Description;
                productVM.Price = product.Price;
                productVM.ListPrice = product.ListPrice;
                productVM.Price50 = product.Price50;
                productVM.Price100 = product.Price100;
                productVM.Author = product.Author;
                productVM.ISBN = product.ISBN;
                productVM.ImageUrl = product.ImageUrl;
                productVM.CategoryId = product.CategoryId;
                return View(productVM);
            }
        }

        // POST
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            // when the form posts it will hit this method
            // 1) check model state
            // 2) add product
            // 3) redirect back to index

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    // if an image in passed in, find and delete it
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    obj.ImageUrl = @"\images\product"+fileName;
                }

                // map our values from the view model to a product, to be saved to db
                Product newProduct = new Product()
                {
                    Title = obj.Title,
                    ISBN = obj.ISBN,
                    Author = obj.Author,
                    Description = obj.Description,
                    ListPrice = obj.ListPrice,
                    Price = obj.Price,
                    Price50 = obj.Price50,
                    Price100 = obj.Price100,
                    CategoryId = obj.CategoryId,
                    ImageUrl = obj.ImageUrl,
                    Id = obj.Id
                };

                if (obj.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(newProduct);
                } else
                {
                    _unitOfWork.ProductRepository.Update(newProduct);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            } else
            {
                obj.CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(obj);
            }
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new
            {
                data = products
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Product productToBeDeleted = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error when deleting" });
            }

            if (productToBeDeleted.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }


            _unitOfWork.ProductRepository.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Successfully deleted product" });
        }

        #endregion

    }
}

