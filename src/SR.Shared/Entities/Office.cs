namespace SR.Shared.Entities;

public record Office
{
	public Guid OfficeId { get; init; }
	public string Name { get; init; }
	public string Address { get; init; }
	public string City { get; init; }
	public string Region { get; init; }
	public string GpsCode { get; init; }
	public string Email { get; init; }
}