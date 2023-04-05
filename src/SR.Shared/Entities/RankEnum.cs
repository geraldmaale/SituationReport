using System.ComponentModel;

namespace SR.Shared.Entities;

public enum RankEnum
{
	[Description("Comptroller")]
	Comptroller,
	[Description("Deputy Comptroller")]
	DeputyComptroller,
	[Description("Commissioner")]
	Commissioner,
	[Description("Deputy Commissioner")]
	DeputyCommissioner,
	[Description("Chief Superintendent")]
	ChiefSuperintendent,
	[Description("Superintendent")]
	Superintendent,
	[Description("Deputy Superintendent")]
	DeputySuperintendent,
	[Description("Inspector")]
	Inspector,
	[Description("Assistant Inspector")]
	AssistantInspector,
	[Description("Immigration Control Officer")]
	ControlOfficer,
	[Description("Immigration Assistant Control Officer I")]
	AssistantControlOfficerI,
	[Description("Immigration Assistant Control Officer II")]
	AssistantControlOfficerII,
}