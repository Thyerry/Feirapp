using Feirapp.Domain.Models;
using FluentValidation;

namespace Feirapp.Service.Validators.GroceryItemValidators;

public class CreateGroceryItemValidator : AbstractValidator<GroceryItem>
{
    public CreateGroceryItemValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("O Nome do produto é obrigatório");
        RuleFor(item => item.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("O Preço não pode ter um valor negativo");
    }
}