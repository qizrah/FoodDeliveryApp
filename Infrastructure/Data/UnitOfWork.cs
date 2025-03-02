﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private IGenericRepository<Category>? _categoryRepository;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<Category> Category
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new GenericRepository<Category>(_dbContext);
                }
                return _categoryRepository;
            }
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}