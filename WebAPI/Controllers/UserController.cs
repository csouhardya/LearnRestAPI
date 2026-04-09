using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IConfiguration configuration, IUserService loginService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserService _loginService = loginService;

        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromQuery] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
            {
                // TODO : validate
                return BadRequest();
            }

            if(string.IsNullOrEmpty(request.Password))
            {
                // TODO : validate
                return BadRequest();
            }

            var userResponse = await _loginService.ValidateCredentialsAsync(request);
            if(userResponse.Role == UserRoles.NotRegistered)
            {
                // TODO : validate
                return Unauthorized("Not Registered");
            }

            if(!userResponse.IsValid)
            {
                //TODO : validate
                return Unauthorized();
            }

            var jwt = CreateJwt(request,userResponse);

            return Ok(jwt);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(User request)
        {
            var validation = ValidateUserRegistration(request);
            string errorMessage;
            if(!validation.isValid)
            {
                // TODO : Validate
                errorMessage = validation.errorMessages.Count > 1 ? "More than one requested paramters are missing" : validation.errorMessages.First();
                return BadRequest(errorMessage);
            }

            
            var resp = await _loginService.RegisterUserAsync(request);
            if(resp.IsCreated)
            {
                return Created();
            }
            errorMessage = resp.ErrorMessage ?? "Something went wrong";
            return BadRequest(errorMessage);
        }

        private string CreateJwt(LoginRequest request, LoginResponse userResponse)
        {
            string jwtKey = _configuration.GetValue<string>("Jwt:Key")!; // TODO : move to jwtMW and inject it to contextWriter

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, request.Username),
                new(ClaimTypes.Role, userResponse.Role.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
        private (bool isValid, List<string> errorMessages) ValidateUserRegistration(User request)
        {
            List<string> errors = [];
            bool isValid = false;
            if (string.IsNullOrWhiteSpace(request.FirstName))
                errors.Add("First Name is required.");

            if (string.IsNullOrWhiteSpace(request.LastName))
                errors.Add("Last Name is required.");

            if (request.DateOfBirth == default)
                errors.Add("Date of Birth is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                errors.Add("Email is required.");

            if (request.Credentials == null || string.IsNullOrWhiteSpace(request.Credentials.Username))
                errors.Add("Username is required.");

            if (request.Credentials == null || string.IsNullOrWhiteSpace(request.Credentials.Password))
                errors.Add("Password is required.");

            if (errors.Count == 0)
                isValid = true;

            return (isValid, errors);
        }
    }
}
