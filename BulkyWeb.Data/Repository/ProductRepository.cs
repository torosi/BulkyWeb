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
            _dbSet.Update(obj); // dbSet is an internal field from Repository using type T - which we passed in when we inherited from it
        }
    }
}

