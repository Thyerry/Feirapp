using Feirapp.Domain.Services.GroceryItems.Dtos;
using FluentValidation;

namespace Feirapp.Domain.Services.GroceryItems.Validators;

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