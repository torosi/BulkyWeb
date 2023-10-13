using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    { 
        IEnumerable<T> GetAll();
        T GetFirstOrDefault(Expression<Func<T, bool>> filter); // this is the syntax for a linq expression as a parameter
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
