using FluentValidation;
using SR.Shared.DTOs.Officers;

namespace SR.Shared.Validators;

public class OfficerCreationValidator : AbstractValidator<OfficerCreationDto>
{
	public OfficerCreationValidator()
	{
		RuleFor(o => o.FirstName)
			.NotEmpty()
			.WithMessage("Please specify the first name")
			.MaximumLength(20);

		RuleFor(o => o.LastName)
			.NotEmpty()
			.WithMessage("Please specify the last name")
			.MaximumLength(20);

		RuleFor(o => o.Gender)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please select the gender");

		RuleFor(o => o.PhoneNumber)
			.NotEmpty()
			.WithMessage("Phone number is required")
			.Matches(@"^[0]\d{9}$")
			.WithMessage("Phone number is not valid");

		RuleFor(o => o.Rank)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please select the officer's rank");

		RuleFor(o => o.Username)
			.NotEmpty()
			.WithMessage("User name is required")
			.Matches( @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
			.WithMessage("Username should be a valid email address");
		
		RuleFor(o => o.Password)
			.NotEmpty()
			.WithMessage("Password is required")
			.MinimumLength(6)
			.WithMessage("Password must be at least 6 characters long");
		
		RuleFor(o => o.Email)
			.Matches( @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
			.WithMessage("Email should be a valid email address");

	}
}


public class OfficerUpdateValidator : AbstractValidator<OfficerUpdateDto>
{
	public OfficerUpdateValidator()
	{
		RuleFor(o => o.FirstName)
			.NotEmpty()
			.WithMessage("Please specify the first name")
			.MaximumLength(20);

		RuleFor(o => o.LastName)
			.NotEmpty()
			.WithMessage("Please specify the last name")
			.MaximumLength(20);

		RuleFor(o => o.Gender)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please select the gender");

		RuleFor(o => o.PhoneNumber)
			.NotEmpty()
			.WithMessage("Phone number is required")
			.Matches(@"^[0]\d{9}$")
			.WithMessage("Phone number is not valid");

		RuleFor(o => o.Rank)
			.IsInEnum()
			.NotEmpty()
			.WithMessage("Please select the officer's rank");

		RuleFor(o => o.Username)
			.NotEmpty()
			.WithMessage("User name is required")
			.Matches( @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
			.WithMessage("Username should be a valid email address");
		
		RuleFor(o => o.Email)
			.Matches( @"^[^@\s]+@[^@\s]+\.[^@\s]+$")
			.WithMessage("Email should be a valid email address");
		

	}
}