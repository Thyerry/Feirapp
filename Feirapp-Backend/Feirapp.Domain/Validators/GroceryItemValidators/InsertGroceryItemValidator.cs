using Feirapp.Domain.Dtos;
using FluentValidation;

namespace Feirapp.Domain.Validators.GroceryItemValidators;

public class InsertGroceryItemValidator : AbstractValidator<GroceryItemDto>
{
    public InsertGroceryItemValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("O Nome do produto é obrigatório");
        RuleFor(item => item.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O Preço não pode ter um valor negativo");
    }
}