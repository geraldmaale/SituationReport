using System.ComponentModel.DataAnnotations;

namespace SR.Shared.DTOs.User
{
    public class UserUpdateDto
    {
        public string Id { get; set; }
        
        public string Email { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        public Guid OfficerId { get; set; }

    }
}
