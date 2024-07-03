using FluentValidation;
using Server.Contracts.Abstractions.Authentication;

namespace Server.Application.Validations
{
    public class RegisterRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
               .NotEmpty().WithMessage("Full name is required.")
               .MinimumLength(1).WithMessage("Full name must be a least 6 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");
        }
    }
}
