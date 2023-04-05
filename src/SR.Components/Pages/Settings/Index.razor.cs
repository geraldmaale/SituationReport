using SR.Components.Helpers;

namespace SR.Components.Pages.Settings;

public class SettingsBase : ServiceComponentBase<SettingsBase>
{
	protected SettingsType? SettingsType { get; set; }

	protected void OnPermitTypeClick()
	{
		SettingsType = Settings.SettingsType.PermitType;
	}
	
	protected void OnNationalityClick()
	{
		SettingsType = Settings.SettingsType.Nationality;
	}
	
	protected void OnRevenueTypeClick()
	{
		SettingsType = Settings.SettingsType.RevenueType;
	}
	
	protected void OnOperationTypeClick()
	{
		SettingsType = Settings.SettingsType.OperationType;
	}
	
	protected void OnAshorePassTypeClick()
	{
		SettingsType = Settings.SettingsType.AshorePassType;
	}
	
	protected void OnVesselTypeClick()
	{
		SettingsType = Settings.SettingsType.VesselType;
	}
}

public enum SettingsType
{
	PermitType,
	RevenueType,
	OperationType,
	AshorePassType,
	VesselType,
	Nationality
}