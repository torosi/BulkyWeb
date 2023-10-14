﻿using BulkyWeb.Data.Data;
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
        public ICategoryRepository CategoryRepository { get; private set; }
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
