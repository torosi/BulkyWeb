using System;
using System.Linq.Expressions;
using BulkyWeb.Data.Data;
using BulkyWeb.Data.Repository.IRepository;
using BulkyWeb.Domain.Models;

namespace BulkyWeb.Data.Repository
{
	public class ProductRepository : Repository<Product>, IProductRepository
	{
        // so most of our basic operations are already implemented in repository, so we can extend that also and pass in our type (its using a generic)
        // the only thing we need to pass to the base class (repository) is the application db context, we can grab that here and pass to base in the constructor

        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product obj)
        {
            var objFromDb = _context.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
                _dbSet.Update(objFromDb);
            }

        }
    }
}

