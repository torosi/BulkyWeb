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
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 2) index action method
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.ProductRepository.GetAll().ToList();
            // rather than passing a whole category to view we can pick out the parts we want and create a new object that can be passed
 
            return View(products);
        }

        // 3.1) create action method
        // GET
        public IActionResult Create()
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

            return View(productVM);
        }

        // POST
        [HttpPost]
        public IActionResult Create(ProductVM obj)
        {
            // when the form posts it will hit this method
            // 1) check model state
            // 2) add product
            // 3) redirect back to index

            if (ModelState.IsValid)
            {
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
                    ImageUrl = obj.ImageUrl
                };


                _unitOfWork.ProductRepository.Add(newProduct);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            } else
            {
                // if we return to the view because the model state is not valid, we get an exception
                // the category list is empty, we need to pass a view model back to the view
                // we could create a new one but it is better to update the one we have. this way we dont lose our product values on the page
                obj.CategoryList = _unitOfWork.CategoryRepository
                    .GetAll().Select(u => new SelectListItem
                         {
                             Text = u.Name,
                             Value = u.Id.ToString()
                         });

                return View(obj); // passing in a ProductVM
            }
        }

        // 3.2) Edit action method
        // GET
        public IActionResult Edit(int id)
        {
            // 1) check the id being passed in from index
            // 2) get the product from the db
            // 3) pass it into view

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        // POST
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        // 3.3) Delete action method
        // GET
        public IActionResult Delete(int id)
        {
            // 1) validate the id passed in from index
            // 2) get the product from the db
            // 3) pass into view

            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Product? product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _unitOfWork.ProductRepository.Remove(product);
            _unitOfWork.Save();

            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }

    }
}

