using AIReportRewriter.API.Middleware;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Features.Reports.Services;
using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Infrastructure.AI;
using AIReportRewriter.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


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

app.Run();