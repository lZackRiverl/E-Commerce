using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.Viewmodels;
using MyShop.Utilities;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles=SD.AdminRole)]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;


        [BindProperty]
        public OrderVm OrderVm { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }




        [HttpGet]
        public IActionResult GetData()
        {

            IEnumerable<OrderHeader> orderHeaders;

             orderHeaders = _unitOfWork.OrderHeader.GetAll(IncludeWord: "ApplicationUser");

            return Json(new { data = orderHeaders });

        }



        public IActionResult Details(int orderid)
        {
            OrderVm orderVm = new OrderVm()
            {
                orderHeader =_unitOfWork.OrderHeader.GetFirstOrDefault(u=>u.Id == orderid,IncludeWord:"ApplicationUser"),
                orderDetail=_unitOfWork.OrderDetail.GetAll(x=>x.OrderHeaderId==orderid, IncludeWord:"Product")
            };
            return View(orderVm);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
		public IActionResult UpdateOrderDetails()
		{

            var orderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVm.orderHeader.Id);
            orderfromdb.Name = OrderVm.orderHeader.Name;
			orderfromdb.Phone = OrderVm.orderHeader.Phone;
			orderfromdb.Address = OrderVm.orderHeader.Address;
			orderfromdb.City = OrderVm.orderHeader.City;

            if (OrderVm.orderHeader.Carrier != null)
            {
                orderfromdb.Carrier= OrderVm.orderHeader.Carrier;   
            }

			if (OrderVm.orderHeader.TrackingNumber != null)
			{
				orderfromdb.TrackingNumber = OrderVm.orderHeader.TrackingNumber;
			}

            _unitOfWork.OrderHeader.Update(orderfromdb);
            _unitOfWork.Complete();

            TempData["Update"] = "Item Has Updated Successfully";
            return RedirectToAction("Details", "Order", new {orderid=orderfromdb.Id});


		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
        {
            _unitOfWork.OrderHeader.UpdateOrderStatus(OrderVm.orderHeader.Id,SD.Proccessing,null);
            _unitOfWork.Complete();
			

			TempData["Update"] = "Order Status Has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVm.orderHeader.Id});

		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{

			var orderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVm.orderHeader.Id);
            orderfromdb.TrackingNumber = OrderVm.orderHeader.TrackingNumber;
            orderfromdb.Carrier = OrderVm.orderHeader.Carrier;
            orderfromdb.OrderStatus = SD.Shipped;
            orderfromdb.ShipingDate = DateTime.Now;


            _unitOfWork.OrderHeader.Update(orderfromdb);
			_unitOfWork.Complete();


			TempData["Update"] = "Order  Has Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVm.orderHeader.Id });

		}




        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {


            var orderfromdb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == OrderVm.orderHeader.Id);
            if (orderfromdb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfromdb.PaymentEndIntId
                };

                var service = new RefundService();
                Refund refund = service.Create(option);

                _unitOfWork.OrderHeader.UpdateOrderStatus(orderfromdb.Id, SD.Cancelled, SD.Refund);

            }
            else
            {

				_unitOfWork.OrderHeader.UpdateOrderStatus(orderfromdb.Id, SD.Cancelled, SD.Cancelled);

			}

		
            _unitOfWork.Complete();


            TempData["Update"] = "Order Has Cancelled Successfully";
            return RedirectToAction("Details", "Order", new { orderid = OrderVm.orderHeader.Id });

        }



    }
}
