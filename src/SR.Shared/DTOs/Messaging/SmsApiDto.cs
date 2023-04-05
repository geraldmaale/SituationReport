namespace SR.Shared.DTOs.Messaging;

public record SmsApiDto
{
    public Guid MessagingId { get; set; }
    public string SenderId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class EmailDetails
{
    public const string SenderAddress = "gibselection@greatideasgh.org";
    public const string SenderName = "GIBS Election";
    public const string BusinessNameTag = "GreatIdeas Business Solutions";
    public const string TeamNameTag = "GIBS Election Team";
    public const string Website = "https://greatideasgh.org";
    
}