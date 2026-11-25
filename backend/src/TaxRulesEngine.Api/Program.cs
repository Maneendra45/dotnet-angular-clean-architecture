using Microsoft.EntityFrameworkCore;
using TaxRulesEngine.Api.Middleware;
using TaxRulesEngine.Application.Services;
using TaxRulesEngine.Application.Strategies;
using TaxRulesEngine.Domain.Interfaces;
using TaxRulesEngine.Infrastructure.Persistence;
using TaxRulesEngine.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistence (EF Core In-Memory for immediate local execution; swap to UseSqlServer for production)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("TaxRulesEngineDb"));

// Repositories & Application Services
builder.Services.AddScoped<IRuleRepository, RuleRepository>();
builder.Services.AddScoped<ITaxEvaluationService, TaxEvaluationService>();

// Strategy Registrations
builder.Services.AddSingleton<ITaxCalculationStrategy, FlatCorporateTaxStrategy>();
builder.Services.AddSingleton<ITaxCalculationStrategy, ProgressiveSlabStrategy>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularClient", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Ensure seed data is initialized
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularClient");
app.MapControllers();

app.Run();
