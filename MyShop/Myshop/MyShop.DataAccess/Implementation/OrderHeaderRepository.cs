using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
	{
        private readonly AppDbContext _context;
      
        public OrderHeaderRepository(AppDbContext context):base(context) 
        {
            _context = context;


        }

        public void Update(OrderHeader orderheaders)
        {
            _context.orderHeaders.Update(orderheaders);

        }

		
		public void UpdateOrderStatus(int Id, string OrderStatus, string PaymentStatus)
		{
		   var orderfromdb=_context.orderHeaders.FirstOrDefault(x => x.Id == Id);
            if (orderfromdb != null)
            {
                orderfromdb.OrderStatus = OrderStatus;
				orderfromdb.PaymentDate=DateTime.Now;



				if (PaymentStatus != null)
                {
                    orderfromdb.PaymentStatus = PaymentStatus;
                }
            }
		}
	}
}
