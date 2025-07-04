using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<ProductDto>> GetProducts(ProductFilter filter)
        {
            var query= _productRepository.Get();
            if (filter.Name != null)
            {
                query = query.Where(p => p.Name.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.MinPrice != null)
            {
                query = query.Where(p => p.Price >= filter.MinPrice);
            }

            query = query.Skip(filter.Skip ?? 0).Take(filter.Count ?? 10);
            var listOfProducts = await query.ToListAsync();

            return listOfProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
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

        public async Task<Product?> GetProductEntityById(long id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task UpdateProduct(Product existingProduct, ProductDto productDto)
        {
            _mapper.Map(productDto, existingProduct);

            // Alternatively, you can manually set properties if you prefer:
            //existingProduct.Name = productDto.Name;
            //existingProduct.Price = productDto.Price;
            //existingProduct.Stock = productDto.Stock;

            await _productRepository.Update();
        }

        public async Task DeleteProduct(long id)
        {
            await _productRepository.Delete(id);
        }
    }
}
