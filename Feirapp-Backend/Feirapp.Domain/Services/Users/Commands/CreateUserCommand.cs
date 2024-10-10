using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;

namespace Feirapp.Domain.Services.Users.Commands;

public record CreateUserCommand(string Name, string Email, string Password, string ConfirmPassword)
{
    public void Validate()
    {
        var errors = new List<ValidationFailure>();
        if (string.IsNullOrWhiteSpace(Name))
            errors.Add(new ValidationFailure(nameof(Name), "The user name cannot be null or empty.", Name));
        if(string.IsNullOrWhiteSpace(Email))
            errors.Add(new ValidationFailure(nameof(Email), "The user email cannot be null or empty.", Email));
        if (string.IsNullOrWhiteSpace(Password))
            errors.Add(new ValidationFailure(nameof(Password), "The password cannot be null or empty.", Password));
        if (Password.Length < 8)
            errors.Add(new ValidationFailure(nameof(Password), "The password must be at least 8 characters long.", Password));
        if (!Regex.IsMatch(Password, @"[A-Z]"))
            errors.Add(new ValidationFailure(nameof(Password), "The password must contain at least one uppercase letter.", Password));
        if (!Regex.IsMatch(Password, @"[a-z]"))
            errors.Add(new ValidationFailure(nameof(Password), "The password must contain at least one lowercase letter.", Password));
        if (!Regex.IsMatch(Password, @"[0-9]"))
            errors.Add(new ValidationFailure(nameof(Password), "The password must contain at least one digit.", Password));
        if (!Regex.IsMatch(Password, @"[\W_]"))
            errors.Add(new ValidationFailure(nameof(Password), "The password must contain at least one special character.", Password));
        if (Password != ConfirmPassword)
            errors.Add(new ValidationFailure(nameof(ConfirmPassword), "The password and confirmation password do not match.", ConfirmPassword));
        
        if (errors.Count > 0)
        {
            throw new ValidationException("Validation failed", errors);
        }
    }
};
