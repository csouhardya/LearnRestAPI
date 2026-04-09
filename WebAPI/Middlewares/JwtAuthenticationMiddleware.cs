using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Middlewares
{
    public class JwtAuthenticationMiddleware(IConfiguration config) : IMiddleware
    {
        private readonly IConfiguration _configuration = config;
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            const string bearerPrefix = "Bearer ";
            string? jwtKey = _configuration.GetValue<string>("Jwt:Key");
            if (string.IsNullOrEmpty(jwtKey))
            {
                //TODO : validate
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                //TODO : validate
                await next(context);
                return;
            }

            if(!authHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            authHeader = authHeader.Substring(bearerPrefix.Length).Trim();
            
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenClaims = handler.ValidateToken(authHeader, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken _);

                var userName = tokenClaims.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    //TODO : validate
                }

                var userRoles = tokenClaims.Claims.Where(_ => _.Type == ClaimTypes.Role).Select(_ => _.Value).ToList();
                if (!userRoles.Any())
                {
                    // TODO : validate
                }

                context.User = tokenClaims;

                await next(context);
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: invalid token");
            }
        }
    }

    public static class JwtAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseJwtAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthenticationMiddleware>();
        }
    }
}
