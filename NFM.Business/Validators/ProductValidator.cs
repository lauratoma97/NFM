using FluentValidation;
using NFM.Business.ModelDTOs;

namespace NFM.Business.Validators
{
    public class ProductValidator : AbstractValidator<BaseProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Price)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please specify a price")
                .GreaterThan(0)
                .WithMessage("Price > 0")
                .PrecisionScale(10, 2, true)
                .WithMessage("Price should have max 2 decimals.");

            RuleFor(p => p.Stock)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please specify a stock")
                .GreaterThan(0)
                .WithMessage("Stock > 0");
        }
    }
}
