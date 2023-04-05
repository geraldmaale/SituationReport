namespace SR.Shared.Responses
{
    public class BlobDownloadResponse
    {
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        public string ContentUrl { get; set; }
        public string FileName { get; set; }
    }
}