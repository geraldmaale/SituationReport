namespace SR.Shared.Entities;

public abstract record EntityBase
{
	public Guid Id { get; init; }
	public DateTime EntryDate { get; init; }
	public Guid OfficerId { get; init; }
	public Officer Officer { get; init; }
}