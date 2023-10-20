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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository 
    {
        private ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}
