﻿using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {



        private readonly AppDbContext _context;
        public ICategoryRepository Category { get; private set; }


        public IProductRepository Product { get; private set; }

        public IshoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

       

       public  IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(AppDbContext context) 
        {
            _context = context;

            Category=new CategoryRepository(context);

            Product =new ProductRepository(context);
            ShoppingCart = new ShoppingCartRepository ( context );
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail= new OrderDetailRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
           
        }

        public int Complete()
        {

            return _context.SaveChanges();
        }

        public void Dispose()
        {
             _context.Dispose();
        }
    }
}
