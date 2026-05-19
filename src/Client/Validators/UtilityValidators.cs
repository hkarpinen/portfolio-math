using Client.Controllers;
using FluentValidation;

namespace Client.Validators;

public sealed class ConvertRequestValidator : AbstractValidator<ConvertRequest>
{
    public ConvertRequestValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty().WithMessage("'from' unit is required.")
            .MaximumLength(20);

        RuleFor(x => x.To)
            .NotEmpty().WithMessage("'to' unit is required.")
            .MaximumLength(20);
    }
}
