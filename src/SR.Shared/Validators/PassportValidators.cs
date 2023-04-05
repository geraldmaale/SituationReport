using FluentValidation;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.Passports;
using SR.Shared.DTOs.Permits;

namespace SR.Shared.Validators;

public class PassportProcessingValidator : AbstractValidator<PassportProcessingManipulationDto>
{
	public PassportProcessingValidator()
	{
		/*RuleFor(o => o.NumberOfNew)
			.NotEmpty()
			.WithMessage("Please enter the number of new passports");

		RuleFor(o => o.NumberOfRenewal)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please enter the number of renewal passports");

		RuleFor(o => o.NumberOfMissing)
			.NotEmpty()
			.WithMessage("Please enter the number of missing/stolen passports");*/
	}
}