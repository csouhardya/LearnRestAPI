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

string? jwtKey = builder.Configuration.GetValue<string>("jwtKey");

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

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero
//    };
//});


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
app.UseJwtAuthenticationMiddleware(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();