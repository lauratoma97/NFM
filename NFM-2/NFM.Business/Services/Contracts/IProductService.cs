using NFM.Business.ModelDTOs;

namespace NFM.Business.Services.Contracts
{
    public interface IProductService
    {
        public Task<List<ProductDto>> GetProducts();

        public Task<ProductDto> GetProductById(long id);

        public Task<long> CreateProduct(CreateProductDto productDto);

        public Task UpdateProduct(UpdateProductDto productDto);

        public Task DeleteProduct(long id);
    }
}
