using System.ComponentModel.DataAnnotations;

namespace SR.Shared.Responses
{
    public class RefreshRequest
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}