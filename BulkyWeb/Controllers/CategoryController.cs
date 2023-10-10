﻿using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Category> catagories = _context.Categories.ToList();
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
                _context.Categories.Add(obj);
                _context.SaveChanges();
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
            Category? categoryFromDb = _context.Categories.Find(id); // find works with the primary key
            //Category? categoryFromDb2 = _context.Categories.FirstOrDefault(u => u.Id == id); // can use on any value not just the primary key
            //Category? categoryFromDb3 = _context.Categories.Where(u => u.Id == id).FirstOrDefault();

            // if there was no category found, return NotFound()
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View();
        }

        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid) 
            {
                _context.Categories.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}
