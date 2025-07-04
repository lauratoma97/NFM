using NFM.Business.ModelDTOs;
using NFM.Domain.Models;

namespace NFM.Business.Services.Contracts
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProducts();

        public Task<ProductDto> GetProductById(long id);

        public Task<long> CreateProduct(CreateProductDto productDto);

        public Task<Product?> GetProductEntityById(long id);

        public Task UpdateProduct(Product existingProduct, ProductDto productDto);

        public Task DeleteProduct(long id);
    }
}
