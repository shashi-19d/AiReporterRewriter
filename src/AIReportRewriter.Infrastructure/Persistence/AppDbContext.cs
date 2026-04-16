using Microsoft.EntityFrameworkCore;
using AIReportRewriter.Domain.Entities;

namespace AIReportRewriter.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<FinancialReport> Reports => Set<FinancialReport>();
}