using Feirapp.Domain.Services.GroceryItems.Dtos;
using FluentValidation;

namespace Feirapp.Domain.Services.GroceryItems.Validators;

public class UpdateGroceryItemValidator : AbstractValidator<GroceryItemDto>
{
    public UpdateGroceryItemValidator()
    {
        RuleFor(item => item.Id)
            .NotEmpty()
            .WithMessage("O Id não pode ser vazio");

        RuleFor(item => item.Name)
            .NotEmpty()
            .WithMessage("O Nome não pode ser vazio");

        RuleFor(item => item.Price)
            .GreaterThan(0)
            .WithMessage("O Preço deve ser maior que 0");

        RuleFor(item => item.StoreName)
            .NotEmpty()
            .WithMessage("O Nome do Mercado não pode ser vazio");
    }
}