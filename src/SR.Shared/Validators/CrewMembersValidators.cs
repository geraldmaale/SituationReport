using FluentValidation;
using SR.Shared.DTOs.Crews;

namespace SR.Shared.Validators;

public class CrewMemberValidator : AbstractValidator<CrewProcessingManipulationDto>
{
	public CrewMemberValidator()
	{
		RuleFor(o => o.OfficerId)
			.NotEmpty()
			.WithMessage("Officer is required");
	}
}

public class EmbarkationValidator : AbstractValidator<EmbarkationDto>
{
	public EmbarkationValidator()
	{
		RuleFor(o => o.NationalityId)
			.NotEmpty()
			.WithMessage("Please select a nationality");

		RuleFor(o => o.Count)
			.NotEmpty()
			.WithMessage("Enter number for nationality");
	}
}

public class DisEmbarkationValidator : AbstractValidator<DisEmbarkationDto>
{
	public DisEmbarkationValidator()
	{
		RuleFor(o => o.NationalityId)
			.NotEmpty()
			.WithMessage("Please select a nationality");

		RuleFor(o => o.Count)
			.NotEmpty()
			.WithMessage("Enter number for nationality");
	}
}