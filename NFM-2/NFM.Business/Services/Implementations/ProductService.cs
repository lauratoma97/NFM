using AutoMapper;
using NFM.Business.ModelDTOs;
using NFM.Business.Services.Contracts;
using NFM.Domain.Models;
using NFM.Domain.Repositories;

namespace NFM.Business.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var productEntities = await _productRepository.Get();
            return productEntities.Select(product => _mapper.Map<ProductDto>(product)).ToList();
        }

        public async Task<ProductDto> GetProductById(long id)
        {
            var productEntityById = await _productRepository.GetById(id);
            if (productEntityById == null)
            {
                return null;
            }
            return _mapper.Map<ProductDto>(productEntityById);
        }

        public async Task<long> CreateProduct(CreateProductDto productDto)
        {
            if (productDto != null)
            {
                var productEntity = _mapper.Map<Product>(productDto);
                await _productRepository.Add(productEntity);
                
                return productEntity.Id;
            }

            return 0;
        }

        public async Task UpdateProduct(UpdateProductDto productDto)
        {
            var productEntity = _mapper.Map<Product>(productDto);
            await _productRepository.Update(productEntity);
        }

        public async Task DeleteProduct(long id)
        {
            await _productRepository.Delete(id);
        }
    }
}
