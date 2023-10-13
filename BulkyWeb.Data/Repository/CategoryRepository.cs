using BulkyWeb.Data.Data;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository // implement our interface, but half the methods are already on Repository so we can grab that aswell so that we dont need to do them again
    {
        private ApplicationDbContext _context;
        // because the repository has the application db context in its ctor, we need to pass it in. This is where it was injecting the dependancy but now because we have implemented it we need to make sure that we pass it through
        // so we can grab it the same using dependancy injection, but call base() on the ctor to pass to all of its base classes
        public CategoryRepository(ApplicationDbContext context) : base(context) // we need to pass the application db context to the repository class that we are implementing 
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Category obj)
        {
            _dbSet.Update(obj);
        }
    }
}
