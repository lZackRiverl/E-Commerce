using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System.Runtime.CompilerServices;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //var cat = _context.Categories.ToList();
            var cat = _unitOfWork.Category.GetAll();
            return View(cat);
        }





        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//security

        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Add(category);
                _unitOfWork.Category.Add(category);
                //_context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Create"] = "Data Has Created Successfully";
                return RedirectToAction("Index");

            }

            return View(category);


        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            //var catInDb=  _context.Categories.Find(id);

            var catInDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            return View(catInDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Update(category);
                _unitOfWork.Category.Update(category);
                // _context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }

            return View(category);
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            // var catInDb = _context.Categories.Find(id);
            var catInDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            return View(catInDb);
        }




        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {


            //var catInDb = _context.Categories.Find(id);
            var catInDb = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (catInDb == null)
            {
                NotFound();
            }

            // _context.Categories.Remove(catInDb);
            _unitOfWork.Category.Remove(catInDb);
            //_context.SaveChanges();
            _unitOfWork.Complete();
            TempData["Delete"] = "Data Has Deleted Successfully";
            return RedirectToAction("Index");
        }







    }
}
