using Server.Contracts.Abstractions.Authentication;
using FluentValidation;

namespace Server.Application.Validations;

public class RefreshTokenUserValidator : AbstractValidator<RefreshTokenUserRequest>
{
    public RefreshTokenUserValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("RefreshToken is required.");
    }
}
