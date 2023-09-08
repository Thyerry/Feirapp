using Feirapp.Domain.Models;
using FluentValidation;

namespace Feirapp.Domain.Validators;

public class InsertGroceryItemValidator : AbstractValidator<GroceryItemModel>
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