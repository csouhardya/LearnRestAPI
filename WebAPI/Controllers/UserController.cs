using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous] // required as httpcontext.user will be empty because of no tokens
    public class UserController(IConfiguration configuration, IUserService loginService) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserService _loginService = loginService;

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
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
            string jwtAud = _configuration.GetValue<string>("Jwt:Audience")!; // TODO : move to jwtMW and inject it to contextWriter
            string jwtIss = _configuration.GetValue<string>("Jwt:Issuer")!; // TODO : move to jwtMW and inject it to contextWriter

            // if claim types.Name is used ms will generate xml based claims e.g. "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
            var claims = new List<Claim>
            {
                new("username", request.Username),
                new("role", "User")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: jwtIss,
                audience: jwtAud,

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
