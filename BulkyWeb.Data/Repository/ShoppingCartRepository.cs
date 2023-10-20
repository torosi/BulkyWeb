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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context) // we need to pass the application db context to the repository class that we are implementing 
        {
            _context = context;
        }

        public void Update(ShoppingCart obj)
        {
            _dbSet.Update(obj);
        }
    }
}
