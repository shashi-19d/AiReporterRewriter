using AIReportRewriter.Domain.Entities;
using AIReportRewriter.Infrastructure.Persistence;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _context;

    public ReportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FinancialReport report)
    {
        await _context.Reports.AddAsync(report);
        await _context.SaveChangesAsync();
    }
}