namespace SR.Shared;

#nullable disable
public class UploadFile
{
    public string FileName { get; set; }
    public string Path { get; set; }

    public Guid Id { get; set; }
}

/// <summary>
/// Configure file paths for the application
/// </summary>
public class FilePaths
{
    public const string Voters = "voters";
    public const string Candidates = "candidates";
    
}