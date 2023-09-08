﻿using Feirapp.Domain.Models;
using FluentValidation;

namespace Feirapp.Domain.Validators;

public class InsertGroceryCategoryValidator : AbstractValidator<GroceryCategoryModel>
{
    public InsertGroceryCategoryValidator()
    {
        RuleFor(item => item.Cest)
            .NotEmpty().WithMessage("O campo CEST é obrigatório")
            .Length(7).WithMessage("CEST inválido: O campo deve conter 11 dígitos");
        RuleFor(item => item.Ncm)
            .NotEmpty().WithMessage("O campo NCM é obrigatório")
            .Length(8).WithMessage("NCM inválido: O campo deve conter exatamente 8 dígitos");
        RuleFor(item => item.ItemNumber)
            .NotEmpty().WithMessage("O campo \"Item\" é obrigatório")
            .MaximumLength(4).WithMessage("Item inválido: O campo está no formato inválido");
    }
}