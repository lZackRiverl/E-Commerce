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
    public class CategoryRepository:GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
      
        public CategoryRepository(AppDbContext context):base(context) 
        {
            _context = context;


        }

        public void Update(Category category)
        {
            var CateoryInDb = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (CateoryInDb != null)
            {
                CateoryInDb.Name = category.Name;
                CateoryInDb.Description = category.Description;
                CateoryInDb.CreatedTime = DateTime.Now;
            }

        }
    }
}
