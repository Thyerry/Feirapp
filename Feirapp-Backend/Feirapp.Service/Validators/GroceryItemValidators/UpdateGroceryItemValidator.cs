using Feirapp.Domain.Enums;
using Feirapp.Domain.Models;
using FluentValidation;

namespace Feirapp.Service.Validators.GroceryItemValidators;

public class UpdateGroceryItemValidator : AbstractValidator<GroceryItem>
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
            .WithMessage("É necessário inserir o Preço");

        RuleFor(item => item.GroceryCategory)
            .NotEqual(item => GroceryCategoryEnum.EMPTY)
            .WithMessage("É necessário selecionar uma categoria");

        RuleFor(item => item.GroceryStoreName)
            .NotEmpty()
            .WithMessage("É necessário inserir o nome do mercado");
    }
}