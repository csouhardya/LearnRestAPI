namespace ApplicationCore.Models
{
    public class LoginResponse
    {
        public bool IsValid { get; set; }
        public UserRoles Role { get; set; }
    }
}
