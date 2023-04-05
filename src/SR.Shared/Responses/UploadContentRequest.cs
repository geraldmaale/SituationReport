namespace SR.Shared.Responses;

public class UploadContentRequest
{
    public string ContentType { get; set; }
        
    public string FileName { get; set; }

    // public Stream? Stream { get; set; }
}