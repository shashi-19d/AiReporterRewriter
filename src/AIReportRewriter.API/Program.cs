using AIReportRewriter.API.Middleware;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Features.Reports.Services;
using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Infrastructure.AI;
using AIReportRewriter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;


var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
// builder.Services.AddScoped<IAIService, MockAIService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddHttpClient<IAIService, HuggingFaceService>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", config =>
    {
        config.PermitLimit = 5; // max 5 requests
        config.Window = TimeSpan.FromSeconds(10); // per 10 sec
        config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        config.QueueLimit = 2;
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); 


app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseRateLimiter();

app.Run();