using FluentValidation;
using NFM.Business.ModelDTOs;
using NFM.Domain.Repositories;

namespace NFM.Business.Validators
{
    public class EmployeeValidator : AbstractValidator<EmployeeDto>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeValidator(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            RuleFor(e => e.CNP)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please specify a CNP.")
                .Length(13)
                .WithMessage("CNP must contain 13 characters.")
                .MustAsync((x, _)=>CnpBeUnique(x))
                .WithMessage("CNP must be unique.")
                .Must(CNPContainDigitsOnly)
                .WithMessage("CNP must contain only digits.");

            RuleFor(e => e.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Please specify a name.")
                .MinimumLength(3)
                .WithMessage("Name should have min 3 characters.")
                .MaximumLength(25)
                .WithMessage("Name should have max 25 characters.");
        }

        private async Task<bool> CnpBeUnique(string cnp)
        {
            return !await _employeeRepository.CnpAlreadyExist(cnp);
        }

        private bool CNPContainDigitsOnly(string cnp)
        {
            foreach (var c in cnp)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
