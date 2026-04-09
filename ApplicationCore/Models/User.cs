using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApplicationCore.Models
{
    public class User
    {
        [JsonIgnore]
        public Guid guid { get; set; }

        [JsonPropertyName("first name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last name")]
        public string LastName { get; set; }

        [JsonPropertyName("date of birth")]
        public DateOnly DateOfBirth { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonIgnore]
        public UserRoles Role { get; set; }

        [JsonPropertyName("credentials")]
        public LoginRequest Credentials {get;set;}
    }
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
