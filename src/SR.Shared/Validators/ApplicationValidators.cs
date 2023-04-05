using FluentValidation;
using SR.Shared.DTOs.Officers;
using SR.Shared.DTOs.Permits;
using SR.Shared.Entities;

namespace SR.Shared.Validators;

public class NationalityTypeValidator : AbstractValidator<NationalityType>
{
	public NationalityTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the nationality")
			.MaximumLength(50);
	}
}

public class VesselTypeValidator : AbstractValidator<VesselType>
{
	public VesselTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the vessel type")
			.MaximumLength(50);
	}
}

public class OperationTypeValidator : AbstractValidator<OperationType>
{
	public OperationTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the operation type")
			.MaximumLength(50);
	}
}

public class AshorePassTypeValidator : AbstractValidator<AshorePassType>
{
	public AshorePassTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the ashore pass type")
			.MaximumLength(50);
	}
}

public class RevenueTypeValidator : AbstractValidator<RevenueType>
{
	public RevenueTypeValidator()
	{
		RuleFor(o => o.Name)
			.NotEmpty()
			.WithMessage("Please specify a name for the revenue type")
			.MaximumLength(50);
	}
}