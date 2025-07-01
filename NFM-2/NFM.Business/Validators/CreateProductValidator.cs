using FluentValidation;
using NFM.Business.ModelDTOs;
using NFM.Domain.Repositories;

namespace NFM.Business.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(p => p.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please specify a name")
                .MinimumLength(3)
                .WithMessage("Name should have min 3 characters.")
                .MaximumLength(25)
                .WithMessage("Name should have max 25 characters.")
                .MustAsync((x, _) => BeUnique(x))
                .WithMessage("Name be unique.");

            Include(new ProductValidator());
        }

        private async Task<bool> BeUnique(string name)
        {
            return !await _productRepository.ProductExist(name);
        }
    }
}
