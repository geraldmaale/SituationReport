using System.ComponentModel.DataAnnotations;

namespace SR.Shared.Params;

public class MessageParams
{
    /// <summary>
    /// Recipient phone number
    /// Must be a valid MSIDSN
    /// Must be in the international telephone number format without the '+' symbol. e.g. 233241234567
    /// </summary>
    [Required(ErrorMessage = "Recipient phone number is required")]
    public string To { get; set; }
        
    /// <summary>
    /// Sender ID (3-11 characters, alphanumeric)
    /// </summary>
    // [Required(ErrorMessage = "Sender's ID is required")]
    [StringLength(11, MinimumLength = 3, ErrorMessage = "Sender's ID should not be more than 11 characters")]
    public string From { get; set; }
        
    /// <summary>
    /// The message to be sent. Must be URL encoded.
    /// </summary>
    [Required(ErrorMessage = "Message content is required")]
    public string Content { get; set; }
        
    /// <summary>
    /// To schedule the message to be sent sometime or date in the future
    /// </summary>
    public string Time { get; set; }
        
    /// <summary>
    /// A unique reference number to identify your transaction for your records
    /// </summary>
    public string Reference { get; set; }
        
    /// <summary>
    /// Delivery report status
    /// </summary>
    public bool Dlr { get; set; } // Delivery report

    /// <summary>
    /// The maximum number of messages to include in the query results. Cannot exceed 100
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    /// The number of messages to skip from the beginning of the query results. Defaults to 0
    /// </summary>
    public int Index { get; set; } = 0;

    /// <summary>
    /// Indicates whether scheduled messages should be included in the query results. This is set to false by default, and only sent messages included.
    /// </summary>
    public bool Pending { get; set; }

    /// <summary>
    /// The date to start querying from.
    /// DateTime (YYYY-MM-DD HH:MM:SS)
    /// </summary>
    public DateTime StartDate { get; set; }
        
    /// <summary>
    /// Last possible date to include in query
    /// DateTime (YYYY-MM-DD HH:MM:SS)
    /// </summary>
    public DateTime EndDate { get; set; }
}

public class SmsResponse
{
    public int Rate { get; set; }
    public string MessageId { get; set; }
    public int Status { get; set; }
    public string NetworkId { get; set; }
    
    
}