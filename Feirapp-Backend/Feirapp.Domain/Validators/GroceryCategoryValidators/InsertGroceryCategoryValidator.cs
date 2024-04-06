using Feirapp.Domain.Dtos;
using FluentValidation;

namespace Feirapp.Domain.Validators.GroceryCategoryValidators;

public class InsertGroceryCategoryValidator : AbstractValidator<GroceryCategoryDto>
{
    public InsertGroceryCategoryValidator()
    {
        RuleFor(item => item.Cest)
            .NotEmpty().WithMessage("O campo CEST é obrigatório")
            .MaximumLength(9).WithMessage("CEST inválido: O campo deve conter 11 dígitos");
        RuleFor(item => item.ItemNumber)
            .NotEmpty().WithMessage("O campo \"Item\" é obrigatório");
    }
}