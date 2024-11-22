using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Viewmodels
{
    public  class OrderVm
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetail { get; set; }

    }
}
