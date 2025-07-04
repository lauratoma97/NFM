﻿using Microsoft.EntityFrameworkCore;
using NFM.Domain.Context;
using NFM.Domain.Models;

namespace NFM.Domain.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MyDbContext _dbContext;

        public ProductRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Product> Get()
        {
            return _dbContext.Products.AsQueryable();
        }

        public async Task<Product> GetById(long id)
        {
            return (await _dbContext.Products.FindAsync(id))!;
        }

        public async Task<long> Add(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            
            return product.Id;
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ProductExist(string name)
        {
            return await _dbContext.Products.AnyAsync(p => p.Name == name);
        }
    }
}
