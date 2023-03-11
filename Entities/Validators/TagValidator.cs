using Entities.DataTransferObjects;
using Entities.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Validators
{
    public class TagValidator : AbstractValidator<TagDto>
    {
        public TagValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Tag name is required")
                .MaximumLength(50).WithMessage("Tag name can't be longer than 50 characters");
            RuleFor(c => c.Description)
                .MaximumLength(250).WithMessage("Tag description can't be longer than 250 characters");
            RuleFor(c => c.Location)
                .MaximumLength(250).WithMessage("Tag description can't be longer than 250 characters");               
            RuleFor(c => c.Attachments)
                .Matches(@"^((http|https):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$");
            RuleFor(c => c.Date)
                .Must(BeAValidDate).WithMessage("Date must be a valid date");
        }

        private bool BeAValidDate(DateTime? arg)
        {
            if (arg == null)
            {
                return true;
            }
            else
            {
                return arg.Value.Date <= DateTime.Now.Date;
            }
        }
    }
}
