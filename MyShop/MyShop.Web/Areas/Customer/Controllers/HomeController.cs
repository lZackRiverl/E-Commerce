using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Implementation;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.Viewmodels;
using MyShop.Utilities;
using System.Security.Claims;
using X.PagedList.Extensions;
//using System.Web.Mvc;
//using System.Web.Mvc;

namespace MyShop.Web.Areas.Customer.Controllers
{

    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int ? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 8;
            var Products = _unitOfWork.Product.GetAll().ToPagedList(pageNumber, pageSize);
            return View(Products);
        }


        public IActionResult Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = _unitOfWork.Product.GetFirstOrDefault(d => d.Id == ProductId, IncludeWord: "category"),
                Count = 1
            };
            
            return View(obj);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingcart)
        {

            var ClaimsIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcart.ApplicationUserId = Claim.Value;


            ShoppingCart cartobj = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                u => u.ApplicationUserId == Claim.Value && u.ProductId == shoppingcart.ProductId
                );
            if (cartobj == null)
            { 
                _unitOfWork.ShoppingCart.Add(shoppingcart);
                _unitOfWork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == Claim.Value).ToList().Count());
                
               
            }
            else
            {
                _unitOfWork.ShoppingCart.IncreaseCount(cartobj, shoppingcart.Count);
                _unitOfWork.Complete();
            }

           





            return RedirectToAction("Index");
        }
    }
}
