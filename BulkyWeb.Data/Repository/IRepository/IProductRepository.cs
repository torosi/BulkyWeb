using System;
using BulkyWeb.Domain.Models;

namespace BulkyWeb.Data.Repository.IRepository
{
	public interface IProductRepository : IRepository<Product>
	{
		void Update(Product obj);
	}
}

