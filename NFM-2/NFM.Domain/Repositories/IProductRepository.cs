using NFM.Domain.Models;

namespace NFM.Domain.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> Get();

        Task<Product> GetById(long id);

        Task<long> Add(Product product);

        Task Update();

        Task Delete(long id);

        Task<bool> ProductExist(string name);
    }
}
