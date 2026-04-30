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
using Serilog;
using Confluent.Kafka;
using ApplicationCore.BackgroundJobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
#region EnvironmentVariables
string jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ApplicationException("JwtKey is missing");
string jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new ApplicationException("JwtIssuer is missing");
string jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new ApplicationException("JwtAudience is missing");
string redisConn = builder.Configuration["Redis_Connection_String"] ?? throw new ApplicationException("Redis connection string is not available.");
string KafkaServer = builder.Configuration["Kafka:BootstrapServers"] ?? throw new ApplicationException("Kafka bootstrap_server is not available");
string KafkaAutoCreateTopics = builder.Configuration["Kafka:AllowAutoCreateTopics"] ?? throw new ApplicationException("Kafka Allow auto_create_topics is not available");
string KafkaAcks = builder.Configuration["Kafka:Acks"] ?? throw new ApplicationException("Kafka acks is not available");
string KafkaMaxMessageRetries = builder.Configuration["Kafka:MessageSendMaxRetries"] ?? throw new ApplicationException("Kafka max_message_retries is not available");
string KafkaRetryBackoff = builder.Configuration["Kafka:RetryBackoffMs"] ?? throw new ApplicationException("Kafka retry_backoff is not available");
string KafkaEnableIdempotence = builder.Configuration["Kafka:EnableIdempotence"] ?? throw new ApplicationException("Kafka enable_idempotence is not available");
#endregion


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConn;
    options.InstanceName = "Products";

});

builder.Host.UseSerilog((context, services, config) =>
{
    config.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext();
});

var config = new ProducerConfig()
{
    BootstrapServers = KafkaServer, // Kafka broker address used by producer to connect to cluster.
    Acks = Enum.TryParse(KafkaAcks, out Acks acksVal) ? acksVal : Acks.None,  // Defines how many broker acknowledgements producer waits for, before considering message successfully delivered.
    // None  -> fire and forget, fastest but unsafe
    // Leader -> leader broker acknowledges
    // All -> all in-sync replicas acknowledge, safest option
    MessageSendMaxRetries = int.TryParse(KafkaMaxMessageRetries, out int maxRetriesVal) ? maxRetriesVal : default, // Number of retry attempts if producer fails temporarily.
    RetryBackoffMs = int.TryParse(KafkaRetryBackoff, out int retryBackOffVal) ? retryBackOffVal : default, // Delay between retry attempts in milliseconds, Prevents aggressive retry spamming
    EnableIdempotence = bool.TryParse(KafkaEnableIdempotence, out bool idempotenceVar) ? idempotenceVar : false // Prevents duplicate message delivery caused by retries.
};

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register MediatR handlers (so ISender/IMediator is available)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetProductsHandler>());
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
        NameClaimType = "username", // need to add as using custom claim instead of using default claimType.name when generating JWT
        RoleClaimType = "role",
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

// So that angular can access it as it runs on diff port otherwise browser restricts communication
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAny", // this name will be registered in UseCors()
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddHostedService<InventoryJob>();
builder.Services.AddHostedService<EmailJob>();
// Register your application service implementation
#region Injections
builder.Services.AddScoped<IConnectionProvider, ConnectionProvider>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<JwtAuthenticationMiddleware>();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddSingleton<IProducer<string, string>>(_ =>
{
    return new ProducerBuilder<string, string>(config).Build();
});
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IKafkaService, KafkaService>();
builder.Services.AddSingleton(config);
#endregion


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
//app.UseJwtAuthenticationMiddleware(); 

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowAny");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();