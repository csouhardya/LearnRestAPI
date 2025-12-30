using ApplicationCore.Interfaces;
using ApplicationCore.Products.Get;
using ApplicationCore.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Register MediatR handlers (so ISender/IMediator is available)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetProductsQueryHandler>());


// Register your application service implementation
builder.Services.AddSingleton<IProductService, ProductService>();

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
