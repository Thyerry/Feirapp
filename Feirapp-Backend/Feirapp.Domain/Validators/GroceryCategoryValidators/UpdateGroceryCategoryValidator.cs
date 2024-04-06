using Feirapp.Domain.Dtos;
using FluentValidation;

namespace Feirapp.Domain.Validators.GroceryCategoryValidators;

public class UpdateGroceryCategoryValidator : AbstractValidator<GroceryCategoryDto>
{
    public UpdateGroceryCategoryValidator()
    {
        RuleFor(item => item.Id)
            .NotEmpty().WithMessage("O campo Id é obrigatório");
        RuleFor(item => item.Cest)
            .NotEmpty().WithMessage("O campo CEST é obrigatório")
            .MaximumLength(9).WithMessage("CEST inválido: O campo deve conter 11 dígitos");
        RuleFor(item => item.Ncm)
            .NotEmpty().WithMessage("O campo NCM é obrigatório");
        RuleFor(item => item.ItemNumber)
            .NotEmpty().WithMessage("O campo \"Item\" é obrigatório");
    }
}