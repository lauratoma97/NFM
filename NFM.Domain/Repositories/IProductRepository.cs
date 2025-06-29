﻿using NFM.Domain.Models;

namespace NFM.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> Get();

        Task<Product> GetById(long id);

        Task<long> Add(Product product);

        Task Update(Product product);

        Task Delete(long id);

        Task<bool> ProductExist(string name);
    }
}
