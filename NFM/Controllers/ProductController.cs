using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NFM.Business.ModelDTOs;
using NFM.Business.Services.Contracts;

namespace NFM.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IValidator<ProductDto> _productValidator;
        private readonly IValidator<CreateProductDto> _createProductValidator;

        public ProductController(IProductService productService, IValidator<ProductDto> productValidator, IValidator<CreateProductDto> createProductValidator)
        {
            _productService = productService;
            _productValidator = productValidator;
            _createProductValidator = createProductValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id}", Name = nameof(GetProductById))]
        public async Task<IActionResult> GetProductById(long id)
        {
            var productById = await _productService.GetProductById(id);
            if (productById == null)
            {
                return NotFound();
            }

            return Ok(productById);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts([FromBody] CreateProductDto product)
        {
            var validationResult = await _createProductValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var createdProductId = await _productService.CreateProduct(product);

            return CreatedAtRoute(nameof(GetProductById), new { id = createdProductId }, createdProductId);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromBody] ProductDto product)
        {
            var validationResult = await _productValidator.ValidateAsync(product);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var productById = await _productService.GetProductById(id);
            if (productById == null)
            {
                return NotFound();
            }

            await _productService.UpdateProduct(product);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var productById = await _productService.GetProductById(id);
            if (productById == null)
            {
                return NotFound();
            }

            await _productService.DeleteProduct(id);
            
            return NoContent();
        }
    }
}