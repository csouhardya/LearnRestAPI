using ApplicationCore.DataAccess;
using ApplicationCore.Interfaces;
using ApplicationCore.Products.Handlers;
using ApplicationCore.Services;
using WebAPI.ConfigurationAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register MediatR handlers (so ISender/IMediator is available)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetQueryHandlers>());


// Register your application service implementation
builder.Services.AddScoped<IConnectionProvider, ConnectionProvider>();
builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
