namespace ApplicationCore.Models
{
    public class LoginResponse
    {
        public bool IsValid { get; set; }
        public UserRoles Role { get; set; }
    }

    public class RegisterResponse
    {
        public bool IsCreated { get; set; }
        public string ErrorMessage { get; set; }
    }
}
