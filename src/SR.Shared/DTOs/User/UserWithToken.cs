namespace SR.Shared.DTOs.User
{
    public class UserWithToken
    {
        /// <summary>
        ///  Represents the PersonId either Staff or Student
        /// </summary>
        public string UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}