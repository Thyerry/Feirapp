using Feirapp.Domain.Models;
using FluentValidation;

namespace Feirapp.Service.Validators.GroceryItemValidators;

public class CreateGroceryItemValidator : AbstractValidator<GroceryItem>
{
    public CreateGroceryItemValidator()
    {
        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("O nome do produto é obrigatório");
        RuleFor(item => item.Price)
            .GreaterThanOrEqualTo(0);
    }
}