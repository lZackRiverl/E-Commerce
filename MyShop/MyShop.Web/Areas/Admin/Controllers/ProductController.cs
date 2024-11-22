using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.Viewmodels;
using System.Runtime.CompilerServices;
//using System.Web.Mvc;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        //for image uploading
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //var cat = _context.Categories.ToList();


            return View();
            //var pro = _unitOfWork.Product.GetAll();
            //return View(pro);
        }



        [HttpGet]
        public IActionResult GetData()
        {


            //var products = _unitOfWork.Product.GetAll(IncludeWord: "Category");
            var products = _unitOfWork.Product.GetAll(IncludeWord: "category");

             return Json(new { data = products });
          //  return Json(products);

        }




        [HttpGet]
        public IActionResult Create()
        {

            ProductVm productvm = new ProductVm()
            {
                product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value=x.Id.ToString()

                })
            };

            return View(productvm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//security

        public IActionResult Create(ProductVm productvm,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //بنشاورله هنا علي الي wwwroot
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload=Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, filename+ext),FileMode.Create)) 
                    {
                        file.CopyTo(filestream);
                    }

                    productvm.product.Image = @"Images\Product\"+filename + ext;
                }
                //_context.Categories.Add(Product);
                _unitOfWork.Product.Add(productvm.product);
                //_context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Create"] = "Data Has Created Successfully";
                return RedirectToAction("Index");

            }

            return View(productvm.product);


        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            //var catInDb=  _context.Categories.Find(id);

          //var prodInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);



            ProductVm productvm = new ProductVm()
            {
                product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };

            return View(productvm);
        //  return View(prodInDb);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Edit(ProductVm productvm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //بنشاورله هنا علي الي wwwroot
                string RootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(file.FileName);


                    if (productvm.product.Image != null)
                    {
                        var oldimg = Path.Combine(RootPath, productvm.product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, filename +ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productvm.product.Image = @"Images\Product\"+filename + ext;
                }

                
                //_context.Categories.Add(Product);
                _unitOfWork.Product.Update(productvm.product);
                //_context.SaveChanges();
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updateed successfully";
                return RedirectToAction("Index");

                //_context.Categories.Update(Product);
                //_unitOfWork.Product.Update(Product);
                // _context.SaveChanges();
                // _unitOfWork.Complete();
                //TempData["Update"] = "Data Has Updated Successfully";
                // return RedirectToAction("Index");
            }

            //return View(Product);
           return View(productvm.product);

        }



    




        [HttpDelete]
        public IActionResult DeleteProduct(int ? id)
        {


            //var catInDb = _context.Products.Find(id);
            var prodInDb = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);

            if (prodInDb == null)
            {
                Json(new { success=false,message="Error While deleting"});
            }

            // _context.Products.Remove(prodInDb);
            _unitOfWork.Product.Remove(prodInDb);


            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath, prodInDb.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            //_context.SaveChanges();
            _unitOfWork.Complete();


          return   Json(new { success = true, message = "Product Has Been deleted Successfully" });

            // TempData["Delete"] = "Data Has Deleted Successfully";
            //  return RedirectToAction("Index");
        }







    }
}
