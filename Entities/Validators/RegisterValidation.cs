using Entities.DataTransferObjects;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must contain 1 uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain 1 lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain 1 number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain 1 non alphanumeric")
                .Must(HasUniqueChars).WithMessage("Password must contain 3 unique characters");
        }

        private bool HasUniqueChars(string arg)
        {
            return arg.Distinct().Count() >= 3;
        }

    }
}
