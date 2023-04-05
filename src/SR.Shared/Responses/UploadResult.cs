namespace SR.Shared.Responses
{
    public class UploadResult
    {
        public bool Uploaded { get; set; }

        public string FileName { get; set; }

        public string StoredFileName { get; set; }

        public string FilePath { get; set; }

        public int ErrorCode { get; set; }
    }
    
    public class UploadResults<T>
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public List<string> Errors { get; set; }
    }
    
    public class UploadResults
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}