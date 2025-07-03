using FluentValidation;
using NFM.Business.ModelDTOs;

namespace NFM.Business.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            Include(new ProductValidator());
        }
    }
}
