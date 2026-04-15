using ApplicationCore.DataAccess;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using WebAPI.ConfigurationAccess;
using WebAPI.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ApplicationCore.Queries.Products.Handlers;
using ApplicationCore.Repositories;
using ApplicationCore.Helpers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();

string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ApplicationException("JwtKey is missing");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new ApplicationException("JwtIssuer is missing");
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new ApplicationException("JwtAudience is missing");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register MediatR handlers (so ISender/IMediator is available)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetQueryHandler>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // disabling because wrong user input will throw default modelstate error not custom errors.
});


// instruction to use JWT auth when app.UseAuthentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Used when ASP.NET needs to read and validate identity, validates token, sets in httpContext.user
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Used when request is unauthorized
}).AddJwtBearer(options => // how to validate JWT
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Ensures token signature is valid (not tampered)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), // The secret key used to verify signature
        ValidateIssuer = true, // validates who created the token
        ValidIssuer = jwtKey, // only valid issuer, should be added in token generation in login controller
        ValidateAudience = true, // validate the scheme for which it was intended for
        ValidAudience = jwtAudience,// only valid audience, should be added in token generation in login controller
        ValidateLifetime = true, // validate if token is expired
        ClockSkew = TimeSpan.Zero // how much extra time allowed
    };
});


// Register your application service implementation
builder.Services.AddScoped<IConnectionProvider, ConnectionProvider>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<JwtAuthenticationMiddleware>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
//app.UseJwtAuthenticationMiddleware(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();