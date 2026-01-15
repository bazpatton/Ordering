using Microsoft.EntityFrameworkCore;
using Ordering.Api.Data;
using Ordering.Api.Services;
using Ordering.Api.Services.Interfaces;
using Ordering.Api.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Port=5432;Database=orderingdb;Username=postgres;Password=postgres";

builder.Services.AddDbContext<OrderingDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register application services
builder.Services.AddSingleton<IEventQueue, InMemoryEventQueue>();
builder.Services.AddScoped<IPaymentProcesser, PaymentProcesser>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderProcessor, OrderProcessor>();

// Register background worker
builder.Services.AddHostedService<OrderProcessingWorker>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
