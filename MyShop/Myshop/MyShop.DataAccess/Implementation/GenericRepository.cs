using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        private readonly AppDbContext _context;
        private DbSet<T> _dbset;
        public GenericRepository(AppDbContext context)
        {
                _context=context;
            _dbset = _context.Set<T>();

              
        }
        public void Add(T entity)
        {
            //_context.categories.add(caegory)
            _dbset.Add(entity);
        }

        //public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, string? IncludeWord)
        //{
        //    IQueryable<T> query = _dbset;
        //    if (predicate != null)
        //    {
        //        query=query.Where(predicate);
        //    }

        //    if (IncludeWord != null)
        //    {
        //        //_context.products.include("category","logos","users")
        //        foreach(var item in IncludeWord.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(item);
        //        }
        //    }
        //    return query.ToList();
        //}






        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate, string IncludeWord = null)
        {
            IQueryable<T> query = _dbset;

            if (!string.IsNullOrEmpty(IncludeWord))
            {
                query = query.Include(IncludeWord);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query.ToList();
        }


        public T GetFirstOrDefault(Expression<Func<T, bool>> predicate, string? IncludeWord)
        {
            IQueryable<T> query = _dbset;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (IncludeWord != null)
            {
                //_context.products.include("category","logos","users")
                foreach (var item in IncludeWord.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.SingleOrDefault();
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbset.RemoveRange(entities);
        }
    }
}
