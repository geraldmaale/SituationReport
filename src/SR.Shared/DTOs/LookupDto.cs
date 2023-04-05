namespace SR.Shared.DTOs;

public class LookupDto
{
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public Guid ElectionId { get; set; }
}