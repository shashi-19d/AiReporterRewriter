using AIReportRewriter.Application.Interfaces;
using AIReportRewriter.Application.Features.Reports.Interfaces;
using AIReportRewriter.Application.Features.Reports.Services;
using AIReportRewriter.Infrastructure.AI;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IAIService, MockAIService>();
builder.Services.AddScoped<IReportService, ReportService>();

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

app.Run();