using BulkyWeb.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository => throw new NotImplementedException();

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
