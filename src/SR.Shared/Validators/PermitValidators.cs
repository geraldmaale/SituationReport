using FluentValidation;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.Permits;

namespace SR.Shared.Validators;

public class PermitTypeValidator : AbstractValidator<PermitTypeManipulationDto>
{
	public PermitTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the permit type")
			.MaximumLength(50);
	}
}

public class PermitProcessingValidator : AbstractValidator<PermitProcessingDetailManipulationDto>
{
	public PermitProcessingValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please enter applicant name")
			.MaximumLength(50);

		RuleFor(o => o.Gender)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please select applicant gender");

		RuleFor(o => o.NationalityId)
			.NotEmpty()
			.WithMessage("Please select applicant nationality");
		
		RuleFor(o => o.PermitTypeId)
			.NotEmpty()
			.WithMessage("Please select permit type");
		
		RuleFor(o => o.Duration)
			.NotEmpty()
			.WithMessage("Please enter permit duration");
		
		RuleFor(o => o.PermitNumber)
			.NotEmpty()
			.WithMessage("Please enter permit number");
	}
}