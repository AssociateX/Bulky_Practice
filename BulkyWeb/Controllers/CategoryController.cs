﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db) 
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();   
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The name cannot exactly match the Display Order.");
            }

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
           
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
            { return NotFound(); 
            }
            Category? categoryFromDb = _categoryRepo.Get(u=>u.Id==id);
            //Category? categoryFromDb1=_db.Categories.FirstOrDefault(u=>u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault(); //not complete
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
          
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            return View(categoryFromDb);
        }
        [HttpPost,ActionName ("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj= _categoryRepo.Get(u => u.Id == id);
            if (obj==null)
            {
                return NotFound();
            }
           _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
           

        }
    }
}
