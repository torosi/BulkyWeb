using BulkyWeb.Data.Data;
using BulkyWeb.Data.Repository;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace BulkyWeb.Web.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ICategoryRepository _unitOfWork.CategoryRepository;
        private readonly IUnitOfWork _unitOfWork; // unit of work has its own category repository so we no longer need to inject it

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> catagories = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(catagories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // custom validators
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("Name", "The display order cannot exactly match the name"); // "Name" here is the field the error is for
            //}

            //if (obj.Name == "test")
            //{
            //    ModelState.AddModelError("", "Name cannot be 'test'"); // leaving first param blank means it will only show in asp-validation-summary = all or ModelOnly
            //}

            if (ModelState.IsValid) // check that the model passed into post method is valid. this checks the validation annotations on the model
            {
                // save to database
                _unitOfWork.CategoryRepository.Add(obj);
                _unitOfWork.Save();
                // temp data is a .net thing that will store what we pass into it, only for the next render. so when we redirect to index we will be able to access our success message/notification
                TempData["success"] = "Category created successfully";
                // redirect to the index action method in this controller to reload the list page
                return RedirectToAction("Index");
            }
            // return the view to stay on the page. this will be when there are errors
            return View();
        }

        // we are passing in the id from the view using asp-route. like this asp-route-id="" - you can put asp-route-whateveryouwant
        public IActionResult Edit(int id) // we need the id of category that the user wants to edit
        {
            // if there was no valid id passed in, return NotFound()
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // get the category from the db
            //Category? categoryFromDb = _context.Categories.Find(id); // find works with the primary key
            //Category? categoryFromDb2 = _context.Categories.FirstOrDefault(u => u.Id == id); // can use on any value not just the primary key
            //Category? categoryFromDb3 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();
            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
           

            // if there was no category found, return NotFound()
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid) 
            {
                _unitOfWork.CategoryRepository.Update(obj);
                _unitOfWork.Save();

                TempData["success"] = "Category created updated";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")] // because the params are the same as the get method, it needs to have a different name but we can say that the action method is still going to be called delete
        public IActionResult DeletePOST(int id)
        {
            Category? obj = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
            if (obj == null) 
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Remove(obj);
            _unitOfWork.Save();

            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }


    }
}
